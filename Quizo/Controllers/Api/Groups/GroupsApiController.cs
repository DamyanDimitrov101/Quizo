using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quizo.Models.Api.Groups;
using Quizo.Services.Groups.Interfaces;
using Quizo.Services.Groups.Models;

namespace Quizo.Controllers.Api.Groups
{
	[Route("api/groups")]
	[ApiController]
	public class GroupsApiController : ControllerBase
	{
		private readonly IGroupsService _groups;

		public GroupsApiController(IGroupsService groups)
		{
			this._groups = groups;
		}


		[HttpGet]
		public async Task<ActionResult<GroupsServiceModel>> All([FromQuery] AllGroupsApiRequestModel query)	
			=>await this._groups.All(new GroupsServiceModel()
			{
				CurrentPage = query.CurrentPage,
				SearchTerm = query.SearchTerm,
				Sorting = query.Sorting
			});
	}
}
