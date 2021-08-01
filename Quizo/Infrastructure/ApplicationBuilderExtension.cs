using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quizo.Data;

namespace Quizo.Infrastructure
{
	public static class ApplicationBuilderExtension
	{
		public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
		{
			using var scopedServices = app.ApplicationServices.CreateScope();
			var data = scopedServices.ServiceProvider.GetService<QuizoDbContext>();
			data.Database.Migrate();
			
			return app;
		}
	}
}
