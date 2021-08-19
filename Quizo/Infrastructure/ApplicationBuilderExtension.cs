using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quizo.Data;
using Quizo.Data.Models.Identity;

using static Quizo.WebConstants;

namespace Quizo.Infrastructure
{
	public static class ApplicationBuilderExtension
	{
		public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
		{
			using var scopedServices = app.ApplicationServices.CreateScope();
			var serviceProvider = scopedServices.ServiceProvider;

			var data = serviceProvider.GetRequiredService<QuizoDbContext>();
			data.Database.Migrate();

			SeedAdministrator(serviceProvider);

			return app;
		}

		private static void SeedAdministrator(IServiceProvider services)
		{
			var userManager = services.GetRequiredService<UserManager<User>>();
			var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();


			Task
				.Run(async () =>
			{
				/*if (await roleManager.RoleExistsAsync(AdministratorRoleName))
				{
					return;
				}
				*/

				var role = new IdentityRole {Name = AdministratorRoleName};
				await roleManager.CreateAsync(role);

				const string adminEmail = "admin@quizo.bg";
				const string adminPassword = "admin12";

				var user = new User { UserName = adminEmail, Email = adminEmail, FullName = "Admin" };
				
				await userManager.CreateAsync(user, adminPassword);

				await userManager.AddToRoleAsync(user, role.Name);
			})
				.GetAwaiter()
				.GetResult();
		}
	}
}
