using JobCreationAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobCreationAPI.Data
{
	public class AppDbContext : IdentityDbContext<ApplicationUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}
		public DbSet<ApplicationUser> Users { get; set; }
		public DbSet<Job> Jobs => Set<Job>();

		protected override void OnModelCreating(
			ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ApplicationUser>()
				.HasIndex(x => x.Email)
				.IsUnique();

			modelBuilder.Entity<Job>()
				.HasOne(x => x.Employer)
				.WithMany(x => x.Jobs)
				.HasForeignKey(x => x.EmployerId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}

