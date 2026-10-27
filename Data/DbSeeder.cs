using JobCreationAPI.Enums;
using JobCreationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobCreationAPI.Data
{
	public class DbSeeder
	{
		public static async Task SeedAsync(AppDbContext context)
		{
			if (await context.Users.AnyAsync())
				return;

			var users = new List<ApplicationUser>
		{
			new()
			{
				FullName = "Approved Employer",
				Email = "approved@example.com",
				Role = "Employer",
				VerificationStatus =
					EmployerVerificationStatus.Approved
			},
			new()
			{
				FullName = "Pending Employer",
				Email = "pending@example.com",
				Role = "Employer",
				VerificationStatus =
					EmployerVerificationStatus.Pending
			},
			new()
			{
				FullName = "Sample Job Seeker",
				Email = "jobseeker@example.com",
				Role = "JobSeeker"
			},
			new()
			{
				FullName = "Sample Administrator",
				Email = "admin@example.com",
				Role = "Administrator"
			}
		};

			await context.Users.AddRangeAsync(users);

			await context.SaveChangesAsync();
		}
	}
}
