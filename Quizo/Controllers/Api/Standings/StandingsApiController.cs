using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
		public async Task<ActionResult<IList<UserViewModel>>> GetStandings(string id)
		{
			var group =await this.data.Groups
				.Include(g => g.Members)
				.FirstOrDefaultAsync(g=> g.Id==id);

				if(group is null) return NotFound();

			var members = group.Members
					.Select(m=> new UserViewModel
					{
						Id = m.Id,
						Name = m.UserName,
						Email = m.Email
					})
					.ToList();

				var standings = new Dictionary<UserViewModel, int>();

				foreach (var member in members)
				{
					var answersCorrect = await this.data.CurrentAnswer
						.Where(ca => 
											ca.UserId == member.Id 
						             && ca.GroupId == group.Id
						             && ca.IsCorect == true)
						.ToListAsync();

					standings.Add(member, answersCorrect.Count);
				}

				var ordered = standings
					.OrderByDescending(x => x.Value)
					.ToDictionary(x => x.Key, x => x.Value); ;
						
				return Ok(ordered.Keys.Select(u=> u.Name).ToList());
		}
	}
}
