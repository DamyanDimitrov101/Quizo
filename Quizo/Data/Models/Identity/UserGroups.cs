using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quizo.Data.Models.Identity
{
	public class UserGroups
	{

		public string UserId { get; set; }
		public User User { get; set; }

		public string GroupId { get; set; }
		public Group Group { get; set; }

	}
}
