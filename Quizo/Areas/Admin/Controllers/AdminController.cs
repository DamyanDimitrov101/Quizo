using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using static Quizo.Areas.Admin.AdminConstants;

namespace Quizo.Areas.Admin.Controllers
{
	[Authorize(Roles = WebConstants.AdministratorRoleName)]
	[Area(AreaName)]
	public abstract class AdminController : Controller
	{
	}
}
