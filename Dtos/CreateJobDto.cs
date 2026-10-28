using System.ComponentModel.DataAnnotations;

namespace JobCreationAPI.Dtos
{
	public class CreateJobDto : IValidatableObject
	{
		[Required]
		[StringLength(150, MinimumLength = 5)]
		public string Title { get; set; } = string.Empty;

		[Required]
		[StringLength(3000, MinimumLength = 20)]
		public string Description { get; set; } = string.Empty;

		[Required]
		[StringLength(150)]
		public string Location { get; set; } = string.Empty;

		[Range(0, 100000000)]
		public decimal? Salary { get; set; }

		[Required]
		[StringLength(2000, MinimumLength = 10)]
		public string Requirements { get; set; } = string.Empty;

		public IEnumerable<ValidationResult> Validate(
			ValidationContext validationContext)
		{
			if (string.IsNullOrWhiteSpace(Title))
			{
				yield return new ValidationResult(
					"Title cannot be blank.",
					new[] { nameof(Title) });
			}

			if (string.IsNullOrWhiteSpace(Description))
			{
				yield return new ValidationResult(
					"Description cannot be blank.",
					new[] { nameof(Description) });
			}

			if (string.IsNullOrWhiteSpace(Location))
			{
				yield return new ValidationResult(
					"Location cannot be blank.",
					new[] { nameof(Location) });
			}

			if (string.IsNullOrWhiteSpace(Requirements))
			{
				yield return new ValidationResult(
					"Requirements cannot be blank.",
					new[] { nameof(Requirements) });
			}
		}
	}
}
