using System.Collections.Generic;
using Quizo.Models.Identity;
using Quizo.Data.Models.Chat;

namespace Quizo.Services.Groups.Models
{
	public class GroupDetailsServiceModel
	{
		public string Id { get; set; }
		public string Name { get; set; }

		public bool IsOwner { get; init; }
		public bool IsJoined { get; set; }
		public bool HasQuestions { get; set; }
		
		public string Description { get; set; }

		public string ImageUrl { get; init; }

		public IList<UserViewModel> Tops { get; set; } = new List<UserViewModel>();
		public IList<Message> LastMessages { get; set; } = new List<Message>();

	}
}
