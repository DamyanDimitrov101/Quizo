using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Quizo.Services.Groups.Interfaces;
using Quizo.Services.Groups.Models;

namespace Quizo.Controllers
{
	[Authorize]
	public class AdminController : Controller
	{
		private readonly IGroupsService service;

		public AdminController(IGroupsService service)
		{
			this.service = service;
		}

		public async Task<IActionResult> AdminGroups()
		{
			var groups = await service.FindAllByIdAsync("");

			return View(groups);
		}
	}
}
