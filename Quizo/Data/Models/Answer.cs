using System;

namespace Quizo.Data.Models
{
	public class Answer
	{
		public string Id { get; init; } = Guid.NewGuid().ToString();

		public string Value { get; set; }

		public string QuestionId { get; init; }
		public Question Question { get; init; }
	}
}
