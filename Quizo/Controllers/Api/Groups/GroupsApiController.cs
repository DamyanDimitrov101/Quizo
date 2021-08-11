using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizo.Data;
using Quizo.Models.Api.Groups;
using Quizo.Models.Groups;
using Quizo.Models.Identity;
using Quizo.Services.Groups;
using Quizo.Services.Groups.Interfaces;
using Quizo.Services.Groups.Models;

namespace Quizo.Controllers.Api.Groups
{
	[Route("api/groups")]
	[ApiController]
	public class GroupsApiController : ControllerBase
	{
		private readonly IGroupsService groups;

		public GroupsApiController(IGroupsService groups)
		{
			this.groups = groups;
		}


		[HttpGet]
		public ActionResult<GroupsServiceModel> All([FromQuery] AllGroupsApiRequestModel query) 
			=> this.groups.All(new GroupListingAllViewModel
			{
				CurrentPage = query.CurrentPage,
				SearchTerm = query.SearchTerm,
				Sorting = query.Sorting
			});
	}
}
