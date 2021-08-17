using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Quizo.Data.DataConstants.Question;

namespace Quizo.Data.Models
{
	public class Question
	{
		public string Id { get; init; } = Guid.NewGuid().ToString();

		[Required]
		[MaxLength(MaxLength)]
		public string Value { get; set; }

		[Required]
		public string  AuthorId { get; init; }
		public string  GroupId { get; init; }

		public IEnumerable<Answer> Answers { get; set; } = new List<Answer>();
	}
}
