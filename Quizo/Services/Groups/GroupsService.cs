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

namespace Quizo.Services.Groups
{
	public class GroupsService : IGroupsService
	{
		private readonly QuizoDbContext _data;
		private readonly UserManager<User> _userManager;

		public GroupsService(QuizoDbContext data, UserManager<User> userManager)
		{
			this._data = data;
			this._userManager = userManager;
		}

		public async Task<ActionResult<GroupsServiceModel>> All([FromQuery] GroupListingAllViewModel query)
		{
			var groupsQuery = this._data
				.Groups
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(query.SearchTerm))
			{
				groupsQuery = groupsQuery.Where(g =>
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

			var groups = await groupsQuery
				.Skip((query.CurrentPage - 1) * GroupListingAllViewModel.GroupsPerPage)
				.Take(GroupListingAllViewModel.GroupsPerPage)
				.Select(g => new GroupListingViewModel
				{
					Id = g.Id,
					Name = g.Name,
					OwnerName = this._data.Users.FirstOrDefault(u => u.Id == g.OwnerId).Email,
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

			return new GroupsServiceModel
			{
				CurrentPage = query.CurrentPage,
				SearchTerm = query.SearchTerm,
				Sorting = query.Sorting,
				Groups = groups,
				TotalGroups = totalGroups
			};
		}

		public async Task<bool> Create(CreateGroupFormModel group, ClaimsPrincipal userPrincipal)
		{
			try
			{
				var user = await _userManager.GetUserAsync(userPrincipal);

				var newGroup = new Group
				{
					Name = group.Name,
					ImageUrl = group.ImageUrl,
					Description = group.Description,
					OwnerId = user.Id,
					Members = new List<User>()
				};

				newGroup.Members.Add(user);
				user.Groups.Add(newGroup);

				_data.Groups.Add(newGroup);
				await _data.SaveChangesAsync();
			}
			catch (Exception e)
			{
				return false;
			}
			return true;
		}

		public async Task<GroupDetailsViewModel> Details(string id, ClaimsPrincipal userPrincipal)
		{
			var currentUser = await this._userManager.GetUserAsync(userPrincipal);
			
			GroupDetailsViewModel groupDetails = await _data.Groups
				.Where(m => m.Id == id)
				.Select(g => new GroupDetailsViewModel
				{
					Id = g.Id,
					Name = g.Name,
					IsOwner = currentUser.Id.Equals(g.OwnerId),
					Description = g.Description,
					ImageUrl = g.ImageUrl,
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
	}
}
