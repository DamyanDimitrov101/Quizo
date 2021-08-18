using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quizo.Models.Questions;
using Quizo.Services.Question.Models;

namespace Quizo.Services.Question.Interfaces
{
	public interface IQuestionService
	{
		Task<bool> Add([FromQuery] AddQuestionFormModel query, ClaimsPrincipal userPrincipal);
		Task<PoolViewModel> All(string id, ClaimsPrincipal userPrincipal);
	}
}
