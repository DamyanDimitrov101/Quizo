using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quizo.Data.Models;
using Quizo.Data.Models.Identity;

namespace Quizo.Data
{
	public class QuizoDbContext : IdentityDbContext<User>
	{
		public DbSet<Question> Questions { get; init; }
		public DbSet<Answer> Answers { get; init; }
		public DbSet<Group> Groups { get; init; }
		public DbSet<UserGroups> UserGroups { get; init; }

		public QuizoDbContext(DbContextOptions<QuizoDbContext> options)
			: base(options)
		{
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserGroups>()
				.HasKey(sc => new { sc.GroupId, sc.UserId});
			
			base.OnModelCreating(modelBuilder);
		}
	}
}
