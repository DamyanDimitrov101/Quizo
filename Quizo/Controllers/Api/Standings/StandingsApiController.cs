using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Quizo.Data;
using Quizo.Models.Identity;

namespace Quizo.Controllers.Api.Standings
{
	[ApiController]
	[Route("api/standings")]
	public class StandingsApiController  : ControllerBase
	{
		private  readonly QuizoDbContext data;

		public StandingsApiController(QuizoDbContext data) 
			=> this.data = data;

		[HttpGet("{id}")]
		public ActionResult<IList<UserViewModel>> GetStandings(string id)
		{
			var group = this.data.Groups
				.Find(id);

				if(group is null) return NotFound();

				var members = data.Users.ToList();
			
				return Ok(members);
		}
	}
}
