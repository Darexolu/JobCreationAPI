using JobCreationAPI.Data;
using JobCreationAPI.Dtos;
using JobCreationAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobCreationAPI.Controllers
{
	[ApiController]
	[Route("api/auth")]
	public class AuthController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IJwtService _jwtService;
		private readonly IWebHostEnvironment _environment;

		public AuthController(
			AppDbContext context,
			IJwtService jwtService,
			IWebHostEnvironment environment)
		{
			_context = context;
			_jwtService = jwtService;
			_environment = environment;
		}	

			[HttpPost("demo-login")]
			public async Task<IActionResult> DemoLogin(
				DemoLoginDto dto,
				CancellationToken cancellationToken)
			{
				if (!_environment.IsDevelopment())
					return NotFound();

				var user = await _context.Users
					.AsNoTracking()
					.FirstOrDefaultAsync(
						x => x.Email == dto.Email,
						cancellationToken);

				if (user is null)
				{
					return Unauthorized(new
					{
						message = "Demo account not found."
					});
				}

			var token = await _jwtService.GenerateToken(user);

			return Ok(new
				{
					accessToken = token,
					tokenType = "Bearer",
					user = new
					{
						user.Id,
						user.FullName,
						user.Email,
						user.Role,
						user.VerificationStatus
					}
				});
			}
		}
	}

