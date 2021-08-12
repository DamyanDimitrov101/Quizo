using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
				.Include(g => g.Members)
				.FirstOrDefaultAsync(g=> g.Id==id);

				if(group is null) return NotFound();

				var members = group.Result.Members.Select(m=> m.FullName).ToList();
			
				return Ok(members);
		}
	}
}
