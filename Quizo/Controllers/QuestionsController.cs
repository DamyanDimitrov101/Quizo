using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizo.Models.Questions;
using Quizo.Services.Question.Interfaces;

namespace Quizo.Controllers
{
	[Authorize]
	public class QuestionsController : Controller
	{
		private readonly IQuestionService _questionService;

		public QuestionsController(IQuestionService questionService)
		{
			this._questionService = questionService;
		}

		public async Task<IActionResult> Pool(string id)
		{
			if (id is null) return NotFound();

			var pool = this._questionService.All(id, this.User);

			if (pool is null) return BadRequest();

			return View(await pool);
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
