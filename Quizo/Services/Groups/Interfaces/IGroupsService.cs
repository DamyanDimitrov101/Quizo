using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quizo.Services.Groups.Models;

namespace Quizo.Services.Groups.Interfaces
{
	public interface IGroupsService
	{
		Task<GroupsServiceModel>  All([FromQuery] GroupsServiceModel query);
		Task<bool> Create([FromQuery] CreateGroupServiceModel query, ClaimsPrincipal userPrincipal);
		Task<GroupDetailsServiceModel> Details(string id, ClaimsPrincipal userPrincipal);
		Task<bool> Join(string groupId, ClaimsPrincipal userPrincipal);
		Task<GroupListingServiceModel> FindAsync(string id);
		Task<bool> EditAsync(GroupListingServiceModel query, ClaimsPrincipal userPrincipal);
		Task<bool> DeleteAsync(string id, ClaimsPrincipal userPrincipal);
		Task<bool> UserIsJoined(string id, ClaimsPrincipal user);
		Task<List<GroupListingServiceModel>> FindAllByIdAsync(string id);
	}
}
