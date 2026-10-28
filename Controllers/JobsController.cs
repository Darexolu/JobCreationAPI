using JobCreationAPI.Data;
using JobCreationAPI.Dtos;
using JobCreationAPI.Enums;
using JobCreationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobCreationAPI.Controllers
{
	[ApiController]
	[Route("api/jobs")]
	public class JobsController : ControllerBase
	{
		private readonly AppDbContext _context;

		public JobsController(AppDbContext context)
		{
			_context = context;
		}

		[HttpPost]
		[Authorize(Roles = "Employer")]
		public async Task<IActionResult> CreateJob(
			[FromBody] CreateJobDto dto,
			CancellationToken cancellationToken)
		{
			// 1. Get the authenticated employer's ID.
			var userId = User.FindFirstValue(
				ClaimTypes.NameIdentifier);

			var employerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrWhiteSpace(employerId))
				return Unauthorized();

			var employer = await _context.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(
					x => x.Id == employerId && x.Role == "Employer",
					cancellationToken);

			if (employer == null)
				return Forbid();

			// 3. Check employer approval status.
			if (employer.VerificationStatus !=
				EmployerVerificationStatus.Approved)
			{
				return StatusCode(403, new
				{
					message =
						"Only approved employers can create jobs."
				});
			}

			// 4. Create the job entity.
			var job = new Job
			{
				Title = dto.Title.Trim(),
				Description = dto.Description.Trim(),
				Location = dto.Location.Trim(),
				Salary = dto.Salary,
				Requirements = dto.Requirements.Trim(),
				EmployerId = employer.Id,
				CreatedAtUtc = DateTime.UtcNow
			};

			// 5. Save the job to SQLite.
			_context.Jobs.Add(job);

			await _context.SaveChangesAsync(
				cancellationToken);

			// 6. Return 201 Created.
			return CreatedAtAction(
				nameof(GetJobById),
				new { id = job.Id },
				new
				{
					job.Id,
					job.Title,
					job.Description,
					job.Location,
					job.Salary,
					job.Requirements,
					job.EmployerId,
					EmployerName = employer.FullName,
					job.CreatedAtUtc
				});
		}

		[HttpGet("{id:guid}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetJobById(
			Guid id,
			CancellationToken cancellationToken)
		{
			var job = await _context.Jobs
				.AsNoTracking()
				.Include(x => x.Employer)
				.FirstOrDefaultAsync(
					x => x.Id == id,
					cancellationToken);

			if (job == null)
			{
				return NotFound(new
				{
					message = "Job not found."
				});
			}

			return Ok(new
			{
				job.Id,
				job.Title,
				job.Description,
				job.Location,
				job.Salary,
				job.Requirements,
				job.EmployerId,
				EmployerName = job.Employer.FullName,
				job.CreatedAtUtc
			});
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetAllJobs(
			CancellationToken cancellationToken)
		{
			var jobs = await _context.Jobs
				.AsNoTracking()
				.OrderByDescending(x => x.CreatedAtUtc)
				.Select(x => new
				{
					x.Id,
					x.Title,
					x.Description,
					x.Location,
					x.Salary,
					x.Requirements,
					x.EmployerId,
					EmployerName = x.Employer.FullName,
					x.CreatedAtUtc
				})
				.ToListAsync(cancellationToken);

			return Ok(jobs);
		}
	}
}