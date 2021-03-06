using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quizo.Models.Questions;
using Quizo.Services.Question.Models;

namespace Quizo.Services.Question.Interfaces
{
	public interface IQuestionService
	{
		Task<bool> Add(AddQuestionFormModel query, ClaimsPrincipal userPrincipal);
		Task<PoolServiceModel> All([FromQuery]PoolServiceModel query, ClaimsPrincipal userPrincipal);
		string GetGroupId(string questionId);
	}
}
