using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quizo.Data;
using Quizo.Data.Models;
using Quizo.Models.Groups;
using Microsoft.AspNetCore.Identity;

namespace Quizo.Controllers
{
	public class GroupsController : Controller
	{
		private readonly QuizoDbContext _context;

		public GroupsController(QuizoDbContext context)
		{
			_context = context;
		}

		// GET: Groups
		public async Task<IActionResult> All()
		{
			var groups = this._context
				.Groups
				.Select(g => new GroupListingAllViewModel
				{
					Name = g.Name,
					 OwnerName= this._context.Users.FirstOrDefault(u=> u.Id==g.OwnerId).Email,
					 Description = g.Description,
					ImageUrl = g.ImageUrl
				})
				.ToListAsync();

			return View(await groups);
		}

		// GET: Groups/Details/5
		public async Task<IActionResult> Details(string id)
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

		// GET: Groups/Create
		[Authorize]
		public IActionResult Create()
		{
			return View();
		}

		// POST: Groups/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Create(CreateGroupFormModel group)
		{
			if (!ModelState.IsValid)
			{
				return View(group);
			}

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var newGroup = new Group
			{
				Name = group.Name,
				ImageUrl = group.ImageUrl,
				Description = group.Description,
				OwnerId = userId
			};

			_context.Add(newGroup);
			await _context.SaveChangesAsync();

			return RedirectToAction("All","Groups");
		}

		// GET: Groups/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
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
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] Group @group)
		{
			if (id != @group.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
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
						throw;
					}
				}
				return RedirectToAction(nameof(All));
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
		[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> DeleteConfirmed(int id)
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
	}
}
