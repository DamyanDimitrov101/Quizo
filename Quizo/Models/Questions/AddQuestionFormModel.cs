﻿using System.ComponentModel.DataAnnotations;
using static Quizo.Data.DataConstants;

namespace Quizo.Models.Questions
{
	public class AddQuestionFormModel
	{
		public string GroupId { get; set; }
		public Data.Models.Group Group { get; set; }

		[Required]
		//[StringLength(Question.MaxLength, MinimumLength = Question.MinLength, ErrorMessage = Question.LengthErrorMessage)]
		public string Value { get; set; }

		[Required]
		//[StringLength(Answer.MaxLength, MinimumLength = Answer.MinLength, ErrorMessage = Answer.LengthErrorMessage)]
		public string AnswerFirst { get; set; }

		[Required]
		//[StringLength(Answer.MaxLength, MinimumLength = Answer.MinLength, ErrorMessage = Answer.LengthErrorMessage)]
		public string AnswerSecond { get; set; }

		[Required]
		//[StringLength(Answer.MaxLength, MinimumLength = Answer.MinLength, ErrorMessage = Answer.LengthErrorMessage)]
		public string AnswerThird { get; set; }

		[Required]
		//[StringLength(Answer.MaxLength, MinimumLength = Answer.MinLength, ErrorMessage = Answer.LengthErrorMessage)]
		public string AnswerFourth { get; set; }

	}
}
