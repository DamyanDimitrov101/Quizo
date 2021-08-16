using System.Collections.Generic;
using Quizo.Data.Models;
using Quizo.Data.Models.Identity;

namespace Quizo.Models.Questions
{
	public class PoolViewModel
	{
		public Group Group { get; set; }
		public string GroupId { get; set; }
		public User User { get; set; }
		public string UserId { get; set; }
		public IEnumerable<Answer> CurrentAnswers { get; set; } = new List<Answer>();
	}
}
