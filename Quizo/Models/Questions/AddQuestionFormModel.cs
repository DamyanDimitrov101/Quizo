using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Quizo.Data.DataConstants;

namespace Quizo.Models.Questions
{
	public class AddQuestionFormModel
	{
		public string GroupId { get; set; }

		[Required]
		[Display(Name = "Your Question here:")]
		[StringLength(Question.MaxLength, MinimumLength = Question.MinLength, ErrorMessage = Question.LengthErrorMessage)]
		public string Value { get; set; }

		[Required]
		[Display(Name="Correct Answer")]
		[StringLength(Answer.MaxLength, MinimumLength = Answer.MinLength, ErrorMessage = Answer.LengthErrorMessage)]
		public string AnswerFirst { get; set; }

		[Required]
		[Display(Name = "Answer")]
		[StringLength(Answer.MaxLength, MinimumLength = Answer.MinLength, ErrorMessage = Answer.LengthErrorMessage)]
		public string AnswerSecond { get; set; }

		[Required]
		[Display(Name = "Answer")]
		[StringLength(Answer.MaxLength, MinimumLength = Answer.MinLength, ErrorMessage = Answer.LengthErrorMessage)]
		public string AnswerThird { get; set; }

		[Required]
		[Display(Name = "Answer")]
		[StringLength(Answer.MaxLength, MinimumLength = Answer.MinLength, ErrorMessage = Answer.LengthErrorMessage)]
		public string AnswerFourth { get; set; }

		public IEnumerable<Answer> Answers { get; set; } = new List<Answer>();

	}
}
