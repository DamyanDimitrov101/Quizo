using System.Collections.Generic;
using Quizo.Models.Groups;

namespace Quizo.Services.Groups.Models
{
	public class GroupsServiceModel
	{
		public const int GroupsPerPage = 4;
		public string SearchTerm { get; set; }

		public GroupSorting Sorting { get; set; }

		public int CurrentPage { get; set; } = 1;
		public int TotalGroups { get; set; }
		public List<GroupListingViewModel> Groups { get; set; }
	}
}
