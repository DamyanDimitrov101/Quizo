using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Quizo.Data.Models;

namespace Quizo.Services.Answers.Interfaces
{
	public interface IAnswerService
	{
		CurrentAnswer GetCurrentAnswer(string questionId, string answerId, ClaimsPrincipal user);
		Task<IList<CurrentAnswer>> GetCurrentAnswers(string currentQuestionId, string userId, List<Data.Models.Question> questions);
	}
}
