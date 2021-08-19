using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quizo.Data;
using Quizo.Data.Models.Identity;
using Quizo.Infrastructure;
using Quizo.Services.Groups;
using Quizo.Services.Groups.Interfaces;
using Quizo.Services.Question;
using Quizo.Services.Question.Interfaces;

namespace Quizo
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
			=> Configuration = configuration;

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddDbContext<QuizoDbContext>(options =>
					options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services
				.AddDatabaseDeveloperPageExceptionFilter();

			services
				.AddDefaultIdentity<User>(options =>
				{
					options.Password.RequireDigit = false;
					options.Password.RequireLowercase = false;
					options.Password.RequireUppercase = false;
					options.Password.RequireNonAlphanumeric = false;
				})
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<QuizoDbContext>();
			services
				.AddControllersWithViews(options 
				=> options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>());

			services
				.AddAuthentication()
				.AddFacebook(fbOptions =>
				{
					fbOptions.AppId = Configuration["Authentication:Facebook:AppId"];
					fbOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
				});

			services.AddTransient<IGroupsService, GroupsService>();
			services.AddTransient<IQuestionService, QuestionService>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.PrepareDatabase();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection()
				.UseStaticFiles()
				.UseRouting()
				.UseAuthentication()
				.UseAuthorization()
				.UseEndpoints(endpoints =>
				{
					endpoints.MapDefaultControllerRoute();
					endpoints.MapRazorPages();
				});

			
		}
	}
}
