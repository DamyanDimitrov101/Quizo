using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizo.Data;
using Quizo.Models.Groups;
using Quizo.Models.Identity;
using Quizo.Services.Groups.Interfaces;
using Quizo.Services.Groups.Models;

namespace Quizo.Services.Groups
{
	public class GroupsService : IGroupsService
	{
		private readonly QuizoDbContext data;

		public GroupsService(QuizoDbContext data) 
			=> this.data = data;

		public GroupsServiceModel All([FromQuery] GroupListingAllViewModel query)
		{
			var groupsQuery = this.data
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

			var groups = groupsQuery
				.Skip((query.CurrentPage - 1) * GroupListingAllViewModel.GroupsPerPage)
				.Take(GroupListingAllViewModel.GroupsPerPage)
				.Select(g => new GroupListingViewModel
				{
					Id = g.Id,
					Name = g.Name,
					OwnerName = this.data.Users.FirstOrDefault(u => u.Id == g.OwnerId).Email,
					Description = g.Description,
					ImageUrl = g.ImageUrl,
					Members = this.data.Users
						.Where(uv => g.Members.Contains(uv))
						.Select(u => new UserViewModel
						{
							Id = u.Id,
							Email = u.Email
						})
						.ToList()
				})
				.ToList();

			return new GroupsServiceModel
			{
				CurrentPage = query.CurrentPage,
				SearchTerm = query.SearchTerm,
				Sorting = query.Sorting,
				Groups = groups,
				TotalGroups = totalGroups
			};
		}
	}
}
