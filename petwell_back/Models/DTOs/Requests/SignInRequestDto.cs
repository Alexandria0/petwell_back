using System.ComponentModel.DataAnnotations;

namespace petwell_back.Models.DTOs.Requests
{
     public class SignInRequestDto
     {
          [Required(ErrorMessage = "Email is required")]
          [EmailAddress(ErrorMessage = "Invalid email format")]
          [StringLength(255)]
          public string Email { get; set; } = string.Empty;

          [Required(ErrorMessage = "Password is required")]
          [StringLength(100)]
          [DataType(DataType.Password)]
          public string Password { get; set; } = string.Empty;
     }
}
