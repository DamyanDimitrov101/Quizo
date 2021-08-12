using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quizo.Data.Models;
using Quizo.Models.Groups;
using Quizo.Services.Groups.Models;

namespace Quizo.Services.Groups.Interfaces
{
	public interface IGroupsService
	{
		Task<ActionResult<GroupsServiceModel>>  All([FromQuery] GroupListingAllViewModel query);
		Task<bool> Create([FromQuery] CreateGroupFormModel query, ClaimsPrincipal userPrincipal);
		Task<GroupDetailsViewModel> Details(string id, ClaimsPrincipal userPrincipal);
		Task<bool> Join(string groupId, ClaimsPrincipal userPrincipal);
	}
}
