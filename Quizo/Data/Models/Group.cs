using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Quizo.Data.Models.Identity;
using Quizo.Models.Identity;
using static Quizo.Data.DataConstants.Group;

namespace Quizo.Data.Models
{
	public class Group
	{
		public string Id { get; init; } = Guid.NewGuid().ToString();

		[Required]
		[StringLength(MaxLength, MinimumLength = MinLength)]
		public string Name { get; set; }

		[Required]
		[StringLength(DescriptionMaxLength, MinimumLength = MinLength)]
		public string Description { get; set; }

		[Required]
		public string OwnerId { get; init; }

		[Display(Name = "Image Url")]
		[Required]
		[Url]
		public string ImageUrl { get; set; }

		public IEnumerable<Question> Questions { get; set; } = new List<Question>();
		public List<User> Members { get; set; } = new List<User>();
	}
}
