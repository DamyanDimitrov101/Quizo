using System.Collections.Generic;
using Quizo.Models.Identity;

namespace Quizo.Models.Groups
{
	public class GroupDetailsViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }

		public string OwnerName { get; init; }

		public string Description { get; set; }

		public string ImageUrl { get; init; }

		public IList<UserViewModel> Tops { get; set; } = new List<UserViewModel>();
	}
}
