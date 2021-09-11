using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizo.Data;
using Quizo.Data.Models;
using Quizo.Data.Models.Identity;
using Quizo.Models.Groups;
using Quizo.Models.Identity;
using Quizo.Services.Groups.Interfaces;
using Quizo.Services.Groups.Models;
using AutoMapper;
using static Quizo.WebConstants;
using Quizo.Data.Models.Chat;

namespace Quizo.Services.Groups
{
	public class GroupsService : IGroupsService
	{
		private readonly QuizoDbContext _data;
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;

		public GroupsService(
			QuizoDbContext data,
			UserManager<User> userManager, 
			IMapper mapper)
		{
			this._data = data;
			this._userManager = userManager;
			this._mapper = mapper;
		}

		public async Task<GroupsServiceModel> All([FromQuery] GroupsServiceModel query)
		{
			var groupsQuery = this._data
				.Groups
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(query.SearchTerm))
			{
				groupsQuery = groupsQuery
					.Where(g =>
						g.Name.ToLower().Contains(query.SearchTerm.ToLower()));
			}

			groupsQuery = query.Sorting switch
			{
				GroupSorting.DateCreated => groupsQuery.OrderByDescending(g => g.Id),
				GroupSorting.Name => groupsQuery.OrderByDescending(g => g.Name),
				GroupSorting.MostMembers => groupsQuery.OrderByDescending(g => g.Members.Count()),
				_ => groupsQuery.OrderByDescending(g => g.Id)
			};

			var totalGroups = groupsQuery.Count();

			var groups = await MapToModel(groupsQuery
				.Skip((query.CurrentPage - 1) * GroupsServiceModel.GroupsPerPage)
				.Take(GroupsServiceModel.GroupsPerPage));

			var groupsViewModel = this._mapper.Map<GroupsServiceModel>(query);
			groupsViewModel.Groups = groups;
			groupsViewModel.TotalGroups = totalGroups;

			return groupsViewModel;
		}

		public async Task<bool> Create(CreateGroupServiceModel group, ClaimsPrincipal userPrincipal)
		{
			try
			{
				var user = await _userManager.GetUserAsync(userPrincipal);

				var newGroupMap = this._mapper.Map<Group>(group);
				newGroupMap.OwnerId = user.Id;
				
				newGroupMap.Members.Add(user);
				user.Groups.Add(newGroupMap);

				_data.Groups.Add(newGroupMap);	
				await _data.SaveChangesAsync();
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public async Task<GroupDetailsServiceModel> Details(string id, ClaimsPrincipal userPrincipal)
		{
			var currentUser = await this._userManager.GetUserAsync(userPrincipal);
			
			GroupDetailsServiceModel groupDetails = await _data.Groups
				.Where(m => m.Id == id)
				.Select(g => new GroupDetailsServiceModel
				{
					Id = g.Id,
					Name = g.Name,
					IsOwner = currentUser.Id.Equals(g.OwnerId),
					IsJoined = g.Members.Contains(currentUser),
					HasQuestions = g.Questions.Any(),
					Description = g.Description,
					ImageUrl = g.ImageUrl,
					LastMessages = this._data.GroupChats
						.FirstOrDefault(m => m.GroupId == g.Id)
						.Messages
						.OrderByDescending(m => m.Time)
						.Take(10)
						.ToList() ?? new List<Message>(),
					Tops = g.Members.OrderBy(m => m.Id).Take(10).Select(u => new UserViewModel
					{
						Email = u.Email,
						Id = u.Id
					})
						.ToList()
				})
				.FirstOrDefaultAsync(m => m.Id == id);


			return groupDetails;
		}

		public async Task<bool> Join(string groupId, ClaimsPrincipal userPrincipal)
		{
			var currentUser = await this._userManager.GetUserAsync(userPrincipal);
			Group @group = await this._data.Groups
				.Include(g => g.Members)
				.FirstOrDefaultAsync(g => g.Id == groupId);

			if (@group is null) return false;

			if (@group.Members .Contains(currentUser)) return false;
			@group.Members.Add(currentUser);
			await this._data.SaveChangesAsync();
			
			return true;
		}

		public async Task<GroupListingServiceModel> FindAsync(string id) =>
			await this._data.Groups
				.Where(g=> g.Id == id)
				.Select(g=>new GroupListingServiceModel()
				{
					Id = g.Id,
					Members = g.Members.Select(u=> new UserViewModel()
					{
						Name = u.FullName,
						Id = u.Id,
						Email = u.Email
					}).ToList(),
					OwnerId = g.OwnerId,
					Description = g.Description,
					ImageUrl = g.ImageUrl,
					Name = g.Name,
					OwnerName = this._data.Users.FirstOrDefault(u=> u.Id == g.OwnerId).FullName,
					Questions = g.Questions.ToList().AsReadOnly()
				})
				.FirstOrDefaultAsync();

		public async Task<bool> EditAsync(GroupListingServiceModel query, ClaimsPrincipal userPrincipal)
		{
			var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
			Group @group = await this._data.Groups
				.FirstOrDefaultAsync(g => g.Id == query.Id);

			if (@group is null) return false;
			if (@group.OwnerId != userId && !userPrincipal.IsInRole("Admin")) return false;

			try
			{
				@group.ImageUrl = query.ImageUrl;
				@group.Description = query.Description;
				@group.Name = query.Name;

				this._data.Update(@group);
				await this._data.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> DeleteAsync(string id, ClaimsPrincipal userPrincipal)
		{
			var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
			
			try
			{
				var @group = await _data.Groups.FindAsync(id);
				
				if (@group is null) return false;
				if (@group.OwnerId != userId && !userPrincipal.IsInRole("Admin")) return false;

				this._data.Groups.Remove(@group);
				await this._data.SaveChangesAsync();
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> UserIsJoined(string id,ClaimsPrincipal user)
		{
			var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			var @group = await _data.Groups
				.Include(g=> g.Members)
				.FirstOrDefaultAsync(g=> g.Id==id);

			if (@group is null) return false;

			if (@group.Members.FirstOrDefault(m=> m.Id == userId) != null)
			{
				return true;
			}

			return false;
		}

		public async Task<List<GroupListingServiceModel>> FindAllByIdAsync(string userId)
		{
			if (userId == "") 
				return await MapToModel(this._data.Groups.AsQueryable());
			
			return await MapToModel(this._data.Groups
				.Where(g => g.OwnerId == userId 
				            || g.Members.Any(u=> u.Id == userId)) 
				.AsQueryable());
		}

		private async Task<List<GroupListingServiceModel>> MapToModel(IQueryable<Group> groups)
			=> await groups.Select(g => new GroupListingServiceModel
				{
					Id = g.Id,
					Name = g.Name,
					OwnerName = this._data.Users.FirstOrDefault(u => u.Id == g.OwnerId).Email,
					OwnerId = this._data.Users.FirstOrDefault(u => u.Id == g.OwnerId).Id,
					Description = g.Description,
					ImageUrl = g.ImageUrl,
					Members = this._data.Users
						.Where(uv => g.Members.Contains(uv))
						.Select(u => new UserViewModel
						{
							Id = u.Id,
							Email = u.Email
						})
						.ToList()
				})
				.ToListAsync();
	}
}
