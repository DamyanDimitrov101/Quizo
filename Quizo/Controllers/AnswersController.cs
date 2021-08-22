using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizo.Data.Models;
using Quizo.Services.Answers.Interfaces;
using Quizo.Services.Question.Interfaces;
using Quizo.Services.Question.Models;

namespace Quizo.Controllers
{
	[Authorize]
	public class AnswersController : Controller
	{
		private readonly IAnswerService _answerService;
		private readonly IQuestionService _questionService;

		public AnswersController(IAnswerService answerService, 
			IQuestionService questionService)
		{
			_answerService = answerService;
			_questionService = questionService;
		}

		[HttpPost]
		public async Task<ActionResult<PoolViewModel>> Respond(PoolViewModel query)
		{
			var pool =await this._questionService.All(query, this.User);
			if (pool is null) return BadRequest();

			pool.CurrentQuestion +=1;
			if (pool.CurrentQuestion >= pool.Questions.Count()) pool.CurrentQuestion = pool.Questions.Count() - 1;

			var currentAnswer = this._answerService.GetCurrentAnswer(query.CurrentQuestionModel.Id, query.CurrentAnswerId, this.User);
			

			return RedirectToAction("Pool", "Questions", pool);   
		}
	}
}
