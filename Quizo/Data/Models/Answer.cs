namespace Quizo.Data.Models
{
	public class Answer
	{
		public int  Id { get; init; }

		public string Value { get; set; }

		public int QuestionId { get; init; }
		public Question Question { get; init; }
	}
}
