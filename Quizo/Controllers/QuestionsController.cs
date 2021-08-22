using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizo.Models.Questions;
using Quizo.Services.Answers.Interfaces;
using Quizo.Services.Question.Interfaces;
using Quizo.Services.Question.Models;

namespace Quizo.Controllers
{
	[Authorize]
	public class QuestionsController : Controller
	{
		private readonly IQuestionService _questionService;
		private readonly IAnswerService _answerService;

		public QuestionsController(IQuestionService questionService, IAnswerService answerService)
		{
			this._questionService = questionService;
			_answerService = answerService;
		}

		public async Task<IActionResult> Pool(PoolServiceModel query)
		{
			if (query is null) return NotFound();

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			
			query.UserId = userId;
			query = await this._questionService.All(query, this.User);
			
			if (query is null) return BadRequest();

			query.CurrentAnswers = await this._answerService.GetCurrentAnswers(query.CurrentQuestionModel.Id, query.UserId, query.Questions.ToList());


			return View(query);
		}

		public async Task<IActionResult> Add(string id)
		{
			if (id is null) return NotFound();

			return View(new AddQuestionFormModel(){GroupId = id});
		}

		// POST: Questions/Add
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Add(AddQuestionFormModel question)
		{
			if (!ModelState.IsValid)
			{
				return View(question);
			}
			
			if (question.GroupId is null) return NotFound();
			
			var isCreated = await this._questionService.Add(question, this.User);

			return isCreated ? RedirectToAction("Details", "Groups", new {Id = question.GroupId})
				: View(question);	
		}

		
	}
}
