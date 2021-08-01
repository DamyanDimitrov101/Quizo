using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quizo.Data.Models;

namespace Quizo.Data
{
	public class QuizoDbContext : IdentityDbContext
	{
		public DbSet<Question> Questions { get; init; }
		public DbSet<Answer> Answers { get; init; }
		public QuizoDbContext(DbContextOptions<QuizoDbContext> options)
			: base(options)
		{
		}
	}
}
