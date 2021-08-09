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
using Quizo.Models.Identity;

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
		public async Task<IActionResult> All([FromQuery] GroupListingAllViewModel query)
		{
			var groupsQuery = this._context
				.Groups
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(query.SearchTerm))
			{
				groupsQuery = groupsQuery.Where(g =>
					g.Name.ToLower().Contains(query.SearchTerm.ToLower()));
			}

			groupsQuery = query.Sorting switch
			{
				GroupSorting.DateCreated => groupsQuery.OrderByDescending(g => g.Id),
				GroupSorting.Name => groupsQuery.OrderByDescending(g => g.Name),
				GroupSorting.MostMembers => groupsQuery.OrderByDescending(g => g.Members.Count()),
				_ => groupsQuery.OrderByDescending(g => g.Id)
			};

			var totalGroups = groupsQuery.Count();

			var groups = groupsQuery
				.Skip((query.CurrentPage - 1) * GroupListingAllViewModel.GroupsPerPage)
				.Take(GroupListingAllViewModel.GroupsPerPage)
				.Select(g => new GroupListingViewModel
				{
					Id = g.Id,
					Name = g.Name,
					OwnerName = this._context.Users.FirstOrDefault(u => u.Id == g.OwnerId).Email,
					Description = g.Description,
					ImageUrl = g.ImageUrl,
					Members = this._context.Users
						.Where(uv => g.Members.Contains(uv))
						.Select(u => new UserViewModel
						{
							Id = u.Id,
							Email = u.Email
						})
						.ToList()
				})
				.ToListAsync();

			query.TotalGroups = totalGroups;
			query.Groups = await groups;

			return View(query);
		}

		// GET: Groups/Details/5
		public async Task<IActionResult> Details(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var groupDetails = await _context.Groups
				.Where(m => m.Id == id)
				.Select(g => new GroupDetailsViewModel
				{
					Id = g.Id,
					Name = g.Name,
					OwnerName = this._context.Users.FirstOrDefault(u => u.Id == g.OwnerId).Email,
					Description = g.Description,
					ImageUrl = g.ImageUrl,

				})
				.FirstOrDefaultAsync(m => m.Id == id);


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

			return RedirectToAction("All", "Groups");
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
