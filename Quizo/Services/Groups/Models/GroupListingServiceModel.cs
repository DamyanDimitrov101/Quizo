using System.Collections.Generic;
using Quizo.Models.Identity;

namespace Quizo.Services.Groups.Models
{
	public class GroupListingServiceModel
	{
		public string Id { get; set; }
		public string Name { get; set; }

		public string OwnerName { get; init; }
		public string OwnerId { get; init; }

		public string Description { get; set; }

		public string ImageUrl { get; init; }

		public IList<UserViewModel> Members { get; set; } = new List<UserViewModel>();

		public IEnumerable<Data.Models.Question> Questions { get; set; } = new List<Data.Models.Question>();
	}
}
