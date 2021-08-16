using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizo.Data;
using Quizo.Data.Models;
using Quizo.Models.Questions;
using Quizo.Services.Question.Interfaces;

namespace Quizo.Controllers
{
	[Authorize]
	public class QuestionsController : Controller
	{
		private readonly IQuestionService _questionService;
		private readonly QuizoDbContext _context;

		public QuestionsController(QuizoDbContext context, IQuestionService questionService)
		{
			this._context = context;
			this._questionService = questionService;
		}

		public async Task<IActionResult> Pool(string id)
		{
			Group @group = await this._context.Groups
				.FirstOrDefaultAsync(g => g.Id == id);

			var @pool = new PoolViewModel()
			{
				Group = @group,
				GroupId = @group.Id
			};

			return View(pool);
		}

		public async Task<IActionResult> Add(string id)
		{
			Group @group = await this._context.Groups
				.FirstOrDefaultAsync(g => g.Id == id);
			var @question =new AddQuestionFormModel()
			{
				Group = @group,
				GroupId = @group.Id
			};
			return View(@question);
		}

		// POST: Questions/Add
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Add([FromQuery]AddQuestionFormModel question)
		{
			var errors = ModelState
				.Where(x => x.Value.Errors.Count > 0)
				.Select(x => new { x.Key, x.Value.Errors })
				.ToArray();

			if (!ModelState.IsValid)
			{
				return View(question);
			}

			var isCreated = await this._questionService.Add(question, this.User);

			return isCreated ? RedirectToAction("Details", "Groups", question.Group.Id)
				: View(question);
		}
	}
}
