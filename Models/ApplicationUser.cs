using JobCreationAPI.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JobCreationAPI.Models
{
	public class ApplicationUser : IdentityUser
	{

		[Required]
		[MaxLength(100)]
		public string FullName { get; set; } = string.Empty;


		[Required]
		[MaxLength(30)]
		public string Role { get; set; } = string.Empty;

		public EmployerVerificationStatus VerificationStatus { get; set; }
			= EmployerVerificationStatus.Pending;

		public ICollection<Job> Jobs { get; set; } = new List<Job>();

	}
}
