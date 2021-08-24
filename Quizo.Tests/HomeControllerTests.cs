using Quizo.Controllers;
using Quizo.Models;

namespace Quizo.Tests
{
	using MyTested.AspNetCore.Mvc;
	using Xunit;
	public class HomeControllerTests
	{
		[Fact]
		public void GetIndexShouldReturnView()
		=> MyMvc
				.Pipeline()
				.ShouldMap(request => request
					.WithPath("/"))
				.To<HomeController>(c => c.Index())
				.Which()
				.ShouldReturn()
				.View();

		[Fact]
		public void GetErrorShouldReturnViewWithCorrectModel()
			=> MyMvc
				.Pipeline()
				.ShouldMap("/Home/Error")
				.To<HomeController>(c => c.Error())
				.Which()
				.ShouldReturn()
				.View(view => view
					.WithModelOfType<ErrorViewModel>());
	}
}
