using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Quizo.Data;
using Quizo.Data.Models;
using Quizo.Services.Answers.Interfaces;

namespace Quizo.Services.Answers
{
	public class AnswerService : IAnswerService
	{
		private readonly QuizoDbContext _data;
		private readonly IMapper _mapper;

		public AnswerService(QuizoDbContext data, IMapper mapper)
		{
			this._data = data;
			this._mapper = mapper;
		}

		public CurrentAnswer GetCurrentAnswer (string questionId, string answerId, string groupId, ClaimsPrincipal user)
		{
			var isCorrect = this.IsTheCorrectAnswer(questionId, answerId);
			var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
			
			var currentAnswer = new CurrentAnswer
			{
				AnswerId = answerId,
				QuestionId = questionId,
				UserId = userId,
				IsCorect = isCorrect,
				GroupId = groupId
			};

			this._data.CurrentAnswer.Add(currentAnswer);
			this._data.SaveChanges();

			return currentAnswer;
		}

		public async Task<IList<CurrentAnswer>> GetCurrentAnswers(string currentQuestionId, string userId, List<Data.Models.Question> questions)
		{
			currentQuestionId ??= questions[0].Id;
			
			return await this._data.CurrentAnswer
				.Where(ca => ca.QuestionId == currentQuestionId
				             && ca.UserId == userId)
				.ToListAsync();
		}

		private bool IsTheCorrectAnswer(string questionId, string answerId)
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
