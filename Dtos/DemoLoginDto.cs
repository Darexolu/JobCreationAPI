using System.ComponentModel.DataAnnotations;

namespace JobCreationAPI.Dtos
{
	public class DemoLoginDto
	{
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		public string Email { get; set; } = string.Empty;
	}
}
