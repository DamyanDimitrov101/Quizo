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
using Quizo.Services.Groups.Interfaces;


namespace Quizo.Controllers
{
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
		public IActionResult All([FromQuery] GroupListingAllViewModel query)
		{
			var service = this._groupsService.All(query);

			query.Groups = service.Groups;
			query.TotalGroups = service.TotalGroups;
			query.SearchTerm= service.SearchTerm;
			query.Sorting = service.Sorting;
			query.CurrentPage = service.CurrentPage;

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
					Tops = g.Members.OrderBy(m => m.Id).Take(10).Select(u => new UserViewModel
					{
						Email = u.Email,
						Id = u.Id
					})
						.ToList()
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
				OwnerId = userId,
				Members = new List<IdentityUser>()
			};

			//TODO:

			/*
			IdentityUser user = new IdentityUser("Test1");
			IdentityUser user1 = new IdentityUser("Test2");
			IdentityUser user2 = new IdentityUser("Test3");
			IdentityUser user3 = new IdentityUser("Test4");
			
			_context.Users.Add(user);
			_context.Users.Add(user1);
			_context.Users.Add(user2);
			_context.Users.Add(user3);*/

			/*
			newGroup.Members = new List<IdentityUser>
				{user, user2, user3, user1};
				*/

			_context.Groups.Add(newGroup);
			await _context.SaveChangesAsync();

			return RedirectToAction("All", "Groups");
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
		[ValidateAntiForgeryToken]
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
						throw;
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
		[ValidateAntiForgeryToken]
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
	}
}
