using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Quizo.Data.Models.Identity
{
	public class User : IdentityUser
	{
		public string FullName { get; set; }
		public IList<Group> Groups { get; set; } = new List<Group>();
	}
}
