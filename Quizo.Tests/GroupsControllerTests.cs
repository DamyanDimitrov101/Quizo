using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyTested.AspNetCore.Mvc;
using Quizo.Controllers;
using Quizo.Data.Models;
using Quizo.Data.Models.Identity;
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


		[Fact]
		public void GetCreateShouldReturnView()
			=> MyMvc
				.Pipeline()
				.ShouldMap(request => request
					.WithPath("/Groups/Create")
					.WithUser())
				.To<GroupsController>(c => c.Create())
				.Which()
				.ShouldReturn()
				.View();

		/*
		[Theory]
		[InlineData(
			"Test Group", 
			"Test Article Description", 
			@"https://www.gameinformer.com/sites/default/files/styles/full/public/2021/07/07/e13a1c25/assassins_creed.jpg")]
		public void PostCreateShouldRedirectIfSuccessfullyCreated(string name, string description, string imageUrl)
			=> MyMvc
				.Pipeline()
				.ShouldMap(request => request
					.WithPath("/Groups/Create")
					.WithMethod(HttpMethod.Post)
					.WithFormFields(new
					{
						Name = name,
						Description = description,
						ImageUrl = imageUrl
					})
					.WithUser(user => user // <---
						.WithUsername("Test Username")
						.WithAuthenticationType("User"))
					.WithAntiForgeryToken())
				.To<GroupsController>(c => c.Create(new CreateGroupServiceModel
				{
					Name = name,
					Description = description,
					ImageUrl = imageUrl
				}))
				.Which()
				.ShouldHave()
				.ValidModelState()
				.AndAlso()
				.ShouldHave()
				.Data(data => data
					.WithSet<Group>(groups =>groups
						.Any(g=> 
							g.Name == name && 
							g.Description == description && 
							g.ImageUrl == imageUrl)))
				.AndAlso()
				.ShouldHave()
				.TempData(tempData => tempData
					.ContainingEntryWithKey(WebConstants.GlobalSuccessMessageKey))
				.AndAlso()
				.ShouldReturn().Redirect(redirect => redirect
					.To<GroupsController>(g => g.All(new GroupsServiceModel())));
					*/


		/*[Fact]
		public void GetDetailsShouldReturnView()
			=> MyMvc
				.Pipeline()
				.ShouldMap(request => request
					.WithPath("/Groups/Details/TestId")
					.WithUser(u=> u.InRole("Admin"))
					.WithAntiForgeryToken())
				.To<GroupsController>(c => c.Details("TestId"))
				.Which(controller => controller
					.WithData(GetAllSortedByMembers()))
				.ShouldReturn()
				.View(view => view
					.WithModelOfType<GroupDetailsServiceModel>()
					.Passing(m => m.Id.Equals("TestId")));*/

	}
}
