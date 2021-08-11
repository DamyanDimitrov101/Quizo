using System.Collections.Generic;
using Quizo.Models.Groups;

namespace Quizo.Models.Api.Groups
{
	public class AllGroupsApiRequestModel
	{
		public string SearchTerm { get; set; }

		public GroupSorting Sorting { get; set; }

		public int CurrentPage { get; set; } = 1;
	}
}
