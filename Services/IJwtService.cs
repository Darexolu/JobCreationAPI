using JobCreationAPI.Models;

namespace JobCreationAPI.Services
{
	public interface IJwtService
	{
		Task<string> GenerateToken(ApplicationUser user);
	}
}
