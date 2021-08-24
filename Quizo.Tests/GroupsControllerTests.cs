using FluentAssertions;
using MyTested.AspNetCore.Mvc;
using Quizo.Controllers;
using Quizo.Models.Groups;
using Quizo.Services.Groups.Models;
using Xunit;

using static Quizo.Tests.Data.GroupsTestData;

namespace Quizo.Tests
{
	public class GroupsControllerTests
	{
		[Fact]
		public void GetAllShouldCheckAuthorizationUserReturnViewWithCorrectModelAndData()
			=> MyPipeline
				.Configuration()
				.ShouldMap(request => request
					.WithPath("/Groups/All")
					.WithQuery(new GroupsServiceModel
					{
							Sorting = GroupSorting.DateCreated
					})
					.WithUser())
				.To<GroupsController>(c=> c.All(new GroupsServiceModel()))
				.Which(controller => controller
					.WithData(GetAll()))
				.ShouldHave()
				.ActionAttributes(attributes => attributes
					.RestrictingForAuthorizedRequests())
				.AndAlso()
				.ShouldReturn()
				.View(view => view
					.WithModelOfType<GroupsServiceModel>()
					.Passing(m=> m.TotalGroups.Should().Be(10)));


		[Fact]
		public void GetAllWithMembersSortingShouldReturnViewWithCorrectData()
			=> MyPipeline
				.Configuration()
				.ShouldMap(request => request
					.WithPath("/Groups/All")
					.WithQuery(new GroupsServiceModel
					{
						Sorting = GroupSorting.MostMembers
					})
					.WithUser())
				.To<GroupsController>(c => c.All(new GroupsServiceModel()))
				.Which(controller => controller
					.WithData(GetAllSortedByMembers()))
				.ShouldReturn()
				.View(view => view
					.WithModelOfType<GroupsServiceModel>()
					.Passing(m => m.Groups[0].Id.Equals("TestId")));
	}
}
