using System.ComponentModel.DataAnnotations;

namespace JobCreationAPI.Models
{
	public class Job
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		[Required]
		[MaxLength(150)]
		public string Title { get; set; } = string.Empty;

		[Required]
		[MaxLength(3000)]
		public string Description { get; set; } = string.Empty;

		[Required]
		[MaxLength(150)]
		public string Location { get; set; } = string.Empty;

		public decimal? Salary { get; set; }

		[Required]
		[MaxLength(2000)]
		public string Requirements { get; set; } = string.Empty;

		[Required]
		public string EmployerId { get; set; } = string.Empty;

		public ApplicationUser Employer { get; set; } = null!;

		public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
	}
}
