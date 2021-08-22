using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
		private readonly IMapper _mapper;

		public QuestionService(QuizoDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}
		
		public async Task<PoolViewModel> All(
			PoolViewModel query, 
			ClaimsPrincipal userPrincipal)
		{
			Group @group = await this._context.Groups
				.FirstOrDefaultAsync(g => g.Id == query.GroupId);
			var userId =userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

			if (@group is null) return null;

			
			query.Group = @group;
			query.UserId = userId;
			query.IsOwner = @group.OwnerId == userId;
			query.Questions =await _context
				.Questions
				.Include(q => q.Answers)
				.Where(q => q.GroupId == query.GroupId)
				.ToListAsync();

			var question = query.Questions.Skip(query.CurrentQuestion).Take(1).FirstOrDefault();
			if (question is null)
			{
				return null;
			}

			query.CurrentQuestionModel = query.Questions.ToList()[query.CurrentQuestion];
			
			var next = query.CurrentQuestion +1;
			var count = query.Questions.Count() - 1;
			if (next >= count) next = count;

			var prev = query.CurrentQuestion - 1;
			if (prev < 0) prev = 0;

			query.NextQuestion = next;
			query.PrevQuestion = prev;

				return query;
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

				var question = this._mapper.Map<Data.Models.Question> (query);
				question.AuthorId = userId;
				
				var answerFirst = CreateAnswer(question, query.AnswerFirst);
				var answerSecond = CreateAnswer(question, query.AnswerSecond);
				var answerThird = CreateAnswer(question, query.AnswerThird);
				var answerFourth = CreateAnswer(question, query.AnswerFourth);
				
				(question.Answers as List<Answer>)?.Add(answerFirst);
				(question.Answers as List<Answer>)?.Add(answerSecond);
				(question.Answers as List<Answer>)?.Add(answerThird);
				(question.Answers as List<Answer>)?.Add(answerFourth);
				
				_context.Questions.Add(question);

				var correctAnswer = this._mapper.Map<CorrectAnswers>(answerFirst);
				correctAnswer.AnswerId = answerFirst.Id;
				correctAnswer.Answer = answerFirst;
				
				_context.CorrectAnswers.Add(correctAnswer);

				await _context.SaveChangesAsync();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
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

		public string GetGroupId(string questionId)
			=> this._context.Questions
				?.Find(questionId)
				.GroupId;
	}
}
