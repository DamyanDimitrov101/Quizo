using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizo.Services.Groups.Interfaces;
using Quizo.Services.Groups.Models;


namespace Quizo.Controllers
{
	[Authorize]
	public class GroupsController : Controller
	{
		private readonly IGroupsService _groupsService;

		public GroupsController(IGroupsService groupsService)
		{
			_groupsService = groupsService;
		}

		// GET: Groups
		[Authorize]
		public async Task<ActionResult<GroupsServiceModel>> All([FromQuery] GroupsServiceModel query)
		{ 
			var service = await this._groupsService.All(query);
			
			return View(service);
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
		public async Task<IActionResult> Create(CreateGroupServiceModel group)
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

			var group = await _groupsService.FindAsync(id);

			if (group == null)
			{
				return NotFound();
			}

			return View(group);
		}

		// POST: Groups/Edit/5
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Edit(GroupListingServiceModel query)
		{
			if (ModelState.IsValid)
			{
				if (!await this._groupsService.EditAsync(query, this.User)) return BadRequest();
				
				return RedirectToAction(nameof(Details),new {query.Id});
			}
			return View(query);
		}

		// GET: Groups/Delete/5
		[Authorize]
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var group = await _groupsService.FindAsync(id);

			if (group == null)
			{
				return NotFound();
			}

			return View(group);
		}

		// POST: Groups/Delete/5
		[HttpPost, ActionName("Delete")]
		[AutoValidateAntiforgeryToken]
		[Authorize]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			if (! await this._groupsService.DeleteAsync(id, this.User)) return BadRequest();

			return RedirectToAction(nameof(All));
		}
		
		// GET: Groups/Join/Id
		[Authorize]
		public async Task<ActionResult> Join(string id)
		{
			if (id == null)
			{
				return NotFound();
			}
			
			return View(new JoinGroupServiceModel
			{
				Id = id, 
				IsAgreed = false, 
				IsJoined = await this._groupsService.UserIsJoined(id,this.User)
			});
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

			if (this.User.IsInRole("Admin"))
			{
				return Unauthorized();
			}

			var isJoined = this._groupsService.Join(id, this.User);

			return await isJoined ? RedirectToAction(nameof(Details), new {Id = id})
				: View(new JoinGroupServiceModel { Id = id, IsAgreed = false, IsJoined = true});
		}

		[Authorize]
		public async Task<IActionResult> MyGroups()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var groups = await _groupsService.FindAllByIdAsync(userId);

			return View(groups);
		}
	}
}
