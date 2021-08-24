using System.Collections.Generic;
using System.Linq;
using Quizo.Data.Models;
using Quizo.Data.Models.Identity;

namespace Quizo.Tests.Data
{
	public static class GroupsTestData
	{
		public static IEnumerable<Group> GetAll()
			=> Enumerable.Range(0, 10).Select(i => new Group
			{
				Name = "Test",
				Description = "Test"
			});

		public static IEnumerable<Group> GetAllSortedByMembers()
		{
			var groups = Enumerable.Range(0, 10).Select(i => new Group
			{
				Name = "Test",
				Description = "Test",
				Members = new List<User>
				{
					new User(),
					new User()
				}
			})
				.ToList();

			var firstGroup = new Group
			{
				Id = "TestId",
				Name = "Test",
				Description = "Test",
				Members = new List<User>
				{
					new User(),
					new User(),
					new User(),
					new User()
				}
			};

			groups.Add(firstGroup);

			return groups;
		}
	}
}
