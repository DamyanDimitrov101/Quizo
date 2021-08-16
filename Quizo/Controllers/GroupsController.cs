using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizo.Data;
using Quizo.Data.Models;
using Quizo.Models.Groups;
using Quizo.Services.Groups.Interfaces;
using Quizo.Services.Groups.Models;


namespace Quizo.Controllers
{
	[Authorize]
	public class GroupsController : Controller
	{
		private readonly IGroupsService _groupsService;
		private readonly QuizoDbContext _context;

		public GroupsController(IGroupsService groupsService, QuizoDbContext context)
		{
			_groupsService = groupsService;
			_context = context;
		}

		// GET: Groups
		[Authorize]
		public async Task<ActionResult<GroupsServiceModel>> All([FromQuery] GroupListingAllViewModel query)
		{
			var service = await this._groupsService.All(query);

			query.Groups = service.Value.Groups;
			query.TotalGroups = service.Value.TotalGroups;
			query.SearchTerm= service.Value.SearchTerm;
			query.Sorting = service.Value.Sorting;
			query.CurrentPage = service.Value.CurrentPage;

			return View(query);
		}

		// GET: Groups/Details/5
		[Authorize]
		public async Task<IActionResult> Details(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var groupDetails =await _groupsService.Details(id, User);

			if (groupDetails == null)
			{
				return NotFound();
			}

			return View(groupDetails);
		}

		// GET: Groups/Create
		[Authorize]
		public IActionResult Create()
		{
			return View();
		}

		// POST: Groups/Create
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Create(CreateGroupFormModel group)
		{
			if (!ModelState.IsValid)
			{
				return View(group);
			}

			var isCreated = await this._groupsService.Create(group, this.User);

			return  isCreated ? RedirectToAction("All", "Groups") 
				: View(group);
		}

		// GET: Groups/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var @group = await _context.Groups.FindAsync(id);
			if (@group == null)
			{
				return NotFound();
			}
			return View(@group);
		}

		// POST: Groups/Edit/5
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Edit(string id, string name, string imageUrl,string description)
		{
			Group @group = await this._context.Groups
				.FirstOrDefaultAsync(g => g.Id == id);

			if (id != @group.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					@group.ImageUrl = imageUrl;
					@group.Description = description;
					@group.Name = name;

					_context.Update(@group);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!GroupExists(@group.Id))
					{
						return NotFound();
					}
					else
					{
						return BadRequest();
					}
				}
				return RedirectToAction(nameof(Details), @group);
			}
			return View(@group);
		}

		// GET: Groups/Delete/5
		[Authorize]
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var @group = await _context.Groups
				.FirstOrDefaultAsync(m => m.Id == id);
			if (@group == null)
			{
				return NotFound();
			}

			return View(@group);
		}

		// POST: Groups/Delete/5
		[HttpPost, ActionName("Delete")]
		[AutoValidateAntiforgeryToken]
		[Authorize]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var @group = await _context.Groups.FindAsync(id);
			_context.Groups.Remove(@group);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(All));
		}

		private bool GroupExists(string id)
		{
			return _context.Groups.Any(e => e.Id == id);
		}

		// GET: Groups/Join/Id
		[Authorize]
		public async Task<ActionResult> Join(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var @group = await _context.Groups.FindAsync(id);
			if (@group == null)
			{
				return NotFound();
			}
			return View(new JoinGroupFormModel{Id = id, IsAgreed = false});
		}

		// POST: Groups/Create/Id
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Join(string id, bool isAgreed)
		{
			//|| !query.IsAgreed
			if (string.IsNullOrEmpty(id))
			{
				return NotFound();
			}

			Group @group = await this._context.Groups
				.FirstOrDefaultAsync(g => g.Id == id);

			if (id != @group.Id)
			{
				return NotFound();
			}

			var isJoined = this._groupsService.Join(id, this.User);

			return await isJoined ? RedirectToAction("Details", "Groups", @group)
				: View(new JoinGroupFormModel { Id = id, IsAgreed = false });
		}
	}
}
