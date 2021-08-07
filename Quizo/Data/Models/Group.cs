﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
		public string ImageUrl { get; init; }

		public IEnumerable<Question> Questions { get; set; } = new List<Question>();
	}
}
