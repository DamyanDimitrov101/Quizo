using System.Security.Claims;

namespace Quizo.Services.Answers.Interfaces
{
	public interface IAnswerService
	{
		bool IsTheCorrectAnswer(string questionId, string answerId);
	}
}
