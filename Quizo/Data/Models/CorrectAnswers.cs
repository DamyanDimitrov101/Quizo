using System;
using System.ComponentModel.DataAnnotations;

namespace Quizo.Data.Models
{
	public class CorrectAnswers
	{
		public string Id { get; init; } = Guid.NewGuid().ToString();

		[Required]
		public string AnswerId { get; set; }
		public Answer Answer { get; set; }
		
		[Required]
		public string QuestionId { get; init; }
		public Question Question { get; init; }
	}
}
