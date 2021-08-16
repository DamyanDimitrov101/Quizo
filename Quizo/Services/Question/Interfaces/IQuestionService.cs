using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quizo.Models.Questions;

namespace Quizo.Services.Question.Interfaces
{
	public interface IQuestionService
	{
		Task<bool> Add([FromQuery] AddQuestionFormModel query, ClaimsPrincipal userPrincipal);
	}
}
