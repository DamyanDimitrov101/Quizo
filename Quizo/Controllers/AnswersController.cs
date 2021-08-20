using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizo.Models.Answers;
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
			var isCorrect =_answerService.IsTheCorrectAnswer(query.CurrentQuestionId, query.CurrentAnswerId);

			var pool =await this._questionService.All(query, this.User);


			//if (groupId is null) return NotFound();

			//_answerService.SetAnswer()

			return RedirectToAction("Pool", "Questions", pool);   
		}
	}
}
