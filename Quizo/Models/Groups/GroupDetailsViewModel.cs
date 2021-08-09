using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quizo.Models.Groups
{
	public class GroupDetailsViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }

		public string OwnerName { get; init; }

		public string Description { get; set; }

		public string ImageUrl { get; init; }
		
	}
}
