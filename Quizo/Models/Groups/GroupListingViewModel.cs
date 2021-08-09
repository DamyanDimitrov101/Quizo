using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quizo.Models.Identity;

namespace Quizo.Models.Groups
{
	public class GroupListingViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }

		public string OwnerName { get; init; }

		public string Description { get; set; }

		public string ImageUrl { get; init; }

		public IList<UserViewModel> Members { get; set; } = new List<UserViewModel>();
	}
}
