using JobCreationAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobCreationAPI.Services
{
	public class JwtService : IJwtService
	{
		private readonly IConfiguration _configuration;

		public JwtService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public Task<string> GenerateToken(ApplicationUser user)
		{
			var claims = new List<Claim>
			{
				new Claim(
					JwtRegisteredClaimNames.Sub,
					user.Id.ToString()),

				new Claim(
					JwtRegisteredClaimNames.Email,
					user.Email),

				new Claim(
					JwtRegisteredClaimNames.Jti,
					Guid.NewGuid().ToString()),

				new Claim(
					ClaimTypes.NameIdentifier,
					user.Id.ToString()),

				new Claim(
					ClaimTypes.Name,
					user.FullName),

				new Claim(
					ClaimTypes.Role,
					user.Role)
			};

			var key = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(
					_configuration["Jwt:Key"]!));

			var credentials = new SigningCredentials(
				key,
				SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(
					Convert.ToDouble(
						_configuration["Jwt:DurationInMinutes"])),
				signingCredentials: credentials
			);

			return Task.FromResult(
				new JwtSecurityTokenHandler().WriteToken(token));
		}
	}

}
