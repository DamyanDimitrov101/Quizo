using System.Linq;
using Quizo.Data;
using Quizo.Services.Answers.Interfaces;

namespace Quizo.Services.Answers
{
	public class AnswerService : IAnswerService
	{
		private readonly QuizoDbContext _data;

		public AnswerService(QuizoDbContext data)
		{
			_data = data;
		}

		public bool IsTheCorrectAnswer(string questionId, string answerId)
		{
			var question = _data.Questions.Find(questionId);
			var answer = _data.Answers.Find(answerId);

			if (question is null) return false;
			if (answer is null) return false;

			var correctAnswer = _data.CorrectAnswers
				.FirstOrDefault(ca => ca.AnswerId == answer.Id 
				                      && ca.QuestionId == question.Id);

			return correctAnswer != null;
		}
	}
}
