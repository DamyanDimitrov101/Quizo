using System;
using System.ComponentModel.DataAnnotations;
using Quizo.Data.Models.Identity;

namespace Quizo.Data.Models
{
	public class CurrentAnswer
	{
		public string Id { get; init; } = Guid.NewGuid().ToString();

		[Required]
		public string AnswerId { get; set; }
		public Answer Answer { get; set; }

		[Required]
		public string QuestionId { get; set; }
		public Question Question { get; set; }

		public bool IsCorect { get; set; } = false;

		[Required]
		public string UserId { get; init; }
		public User User { get; init; }
	}
}
