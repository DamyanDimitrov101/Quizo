using System.Collections.Generic;
using Quizo.Data.Models;
using Quizo.Data.Models.Identity;

namespace Quizo.Services.Question.Models
{
	public class PoolViewModel
	{
		public Group Group { get; set; }
		public string GroupId { get; set; }
		public User User { get; set; }
		public string UserId { get; set; }
		public bool IsOwner { get; set; }
		public int CurrentQuestion { get; set; } = 0;
		public Data.Models.Question CurrentQuestionModel { get; set; }
		public int NextQuestion { get; set; }
		public int PrevQuestion { get; set; }
		public string CurrentAnswerId { get; set; }

		public IEnumerable<Data.Models.Question> Questions { get; set; } = new List<Data.Models.Question>();
		public IEnumerable<CurrentAnswer> CurrentAnswers { get; set; } = new List<CurrentAnswer>();
	}
}
