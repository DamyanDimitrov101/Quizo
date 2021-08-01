using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Quizo.Data.DataConstants.Question;

namespace Quizo.Data.Models
{
	public class Question
	{
		public int Id { get; init; }

		[Required]
		[MaxLength(MaxLength)]
		public string Value { get; set; }

		[Required]
		public int  AuthorId { get; init; }

		public IEnumerable<Answer> Answers { get; set; } = new List<Answer>();
	}
}
