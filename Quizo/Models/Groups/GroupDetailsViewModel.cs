using System.Collections.Generic;
using Quizo.Data.Models.Identity;
using Quizo.Models.Identity;

namespace Quizo.Models.Groups
{
	public class GroupDetailsViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }

		public bool IsOwner { get; init; }
		
		public string Description { get; set; }

		public string ImageUrl { get; init; }

		public IList<UserViewModel> Tops { get; set; } = new List<UserViewModel>();
	}
}
