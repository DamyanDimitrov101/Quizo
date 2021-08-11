using Microsoft.AspNetCore.Mvc;
using Quizo.Models.Groups;
using Quizo.Services.Groups.Models;

namespace Quizo.Services.Groups.Interfaces
{
	public interface IGroupsService
	{
		GroupsServiceModel All([FromQuery] GroupListingAllViewModel query);
	}
}
