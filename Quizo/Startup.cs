using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quizo.Data;
using Quizo.Data.Models.Identity;
using Quizo.Infrastructure;
using Quizo.Services.Answers;
using Quizo.Services.Answers.Interfaces;
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

			services.ConfigureApplicationCookie(options =>
			{
				options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
				{
					OnRedirectToLogin = ctx =>
					{
						var requestPath = ctx.Request.Path;
						if (requestPath.Value == "/Home/About")
						{
							ctx.Response.Redirect("/Home/UserLogin");
						}
						else if (requestPath.Value == "/Home/Contact")
						{
							ctx.Response.Redirect("/Home/AdminLogin");
						}

						return Task.CompletedTask;
					}
				};

				options.AccessDeniedPath = new PathString("/Account/AccessDenied");
				options.Cookie.Name = "Cookie";
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
				options.LoginPath = new PathString("/Account/Login");
				options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
				options.SlidingExpiration = true;
			});

			services.AddAutoMapper(typeof(Startup));

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
			services.AddTransient<IAnswerService, AnswerService>();
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
					endpoints.MapDefaultAreaRoute();
					endpoints.MapDefaultControllerRoute();
					endpoints.MapRazorPages();
				});
		}
	}
}
