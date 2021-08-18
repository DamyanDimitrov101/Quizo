using System.Collections.Generic;
using Quizo.Services.Groups.Models;

namespace Quizo.Models.Groups
{
	public class GroupListingAllViewModel
	{
		public const int GroupsPerPage = 4;
		public string SearchTerm { get; set; }

		public GroupSorting Sorting { get; set; }

		public int CurrentPage { get; set; } = 1;
		public int TotalGroups { get; set; }
		public List<GroupListingServiceModel> Groups { get; set; }
	}
}
