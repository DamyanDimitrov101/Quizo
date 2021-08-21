using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quizo.Services.Groups.Interfaces;

namespace Quizo.Areas.Admin.Controllers
{
	public class GroupsController : AdminController
	{
		private readonly IGroupsService service;

		public GroupsController(IGroupsService service)
		{
			this.service = service;
		}

		public async Task<IActionResult> Index()
		{
			var groups = await service.FindAllByIdAsync("");

			return View(groups);
		}
	}
}
