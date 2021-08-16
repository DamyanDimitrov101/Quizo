using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quizo.Data;
using Quizo.Data.Models;
using Quizo.Models.Questions;
using Quizo.Services.Question.Interfaces;

namespace Quizo.Services.Question
{
	public class QuestionService : IQuestionService
	{
		private readonly QuizoDbContext _context;

		public QuestionService(QuizoDbContext context)
		{
			_context = context;
		}

		public async Task<bool> Add(AddQuestionFormModel query, ClaimsPrincipal userPrincipal)
		{
			try
			{
				var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

				if (userId != query.Group.OwnerId)
				{
					return false;
				}

				var question = new Data.Models.Question()
				{
					Value = query.Value,
					Answers = new List<Answer>(),
					AuthorId = userId
				};

				var answerFirst = CreateAnswer(question, query);
				var answerSecond = CreateAnswer(question, query);
				var answerThird = CreateAnswer(question, query);
				var answerFourth = CreateAnswer(question, query);
				

				(question.Answers as List<Answer>)?.Add(answerFirst);
				(question.Answers as List<Answer>)?.Add(answerSecond);
				(question.Answers as List<Answer>)?.Add(answerThird);
				(question.Answers as List<Answer>)?.Add(answerFourth);


				_context.Questions.Add(question);
				await _context.SaveChangesAsync();
			}
			catch (Exception e)
			{
				return false;
			}
			return true;
		}


		private Answer CreateAnswer(Data.Models.Question question, AddQuestionFormModel query)
		{
			return new Answer()
			{
				Question = question,
				QuestionId = question.Id,
				Value = query.AnswerFirst
			};
		}
	}
}
