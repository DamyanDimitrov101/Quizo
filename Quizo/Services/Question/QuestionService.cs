﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quizo.Data;
using Quizo.Data.Models;
using Quizo.Models.Questions;
using Quizo.Services.Question.Interfaces;
using Quizo.Services.Question.Models;

namespace Quizo.Services.Question
{
	public class QuestionService : IQuestionService
	{
		private readonly QuizoDbContext _context;

		public QuestionService(QuizoDbContext context)
		{
			_context = context;
		}
		
		public async Task<PoolViewModel> All(string id, ClaimsPrincipal userPrincipal)
		{
			Group @group = await this._context.Groups
				.FirstOrDefaultAsync(g => g.Id == id);
			var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

			if (@group is null) return null;

			var pool = new PoolViewModel()
			{
				Group = @group,
				GroupId = @group.Id,
				IsOwner = @group.OwnerId == userId,
				Questions = _context
					.Questions
					.Include(q => q.Answers)
					.Where(q => q.GroupId == id)
					.ToList()
			};

			return pool;
		}

		public async Task<bool> Add(AddQuestionFormModel query, ClaimsPrincipal userPrincipal)
		{
			try
			{
				var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

				var group = _context.Groups.FirstOrDefault(g=>g.Id==query.GroupId);

				if (group==null || userId != group.OwnerId)
				{
					return false;
				}

				var question = new Data.Models.Question()
				{
					Value = query.Value,
					Answers = new List<Answer>(),
					AuthorId = userId,
					GroupId = group.Id
				};

				var answerFirst = CreateAnswer(question, query.AnswerFirst);
				var answerSecond = CreateAnswer(question, query.AnswerSecond);
				var answerThird = CreateAnswer(question, query.AnswerThird);
				var answerFourth = CreateAnswer(question, query.AnswerFourth);
				

				(question.Answers as List<Answer>)?.Add(answerFirst);
				(question.Answers as List<Answer>)?.Add(answerSecond);
				(question.Answers as List<Answer>)?.Add(answerThird);
				(question.Answers as List<Answer>)?.Add(answerFourth);


				_context.Questions.Add(question);
				await _context.SaveChangesAsync();
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}


		private Answer CreateAnswer(Data.Models.Question question,string value) 
			=>new()
			{
				Question = question,
				QuestionId = question.Id,
				Value = value
			};
	}
}
