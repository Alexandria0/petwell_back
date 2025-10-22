using petwell_back.Shared.Common;
using System.ComponentModel.DataAnnotations;

namespace petwell_back.Models.DTOs.Requests
{
     public class SignUpRequestDto
     {
          [Required(ErrorMessage = "Name is required")]
          [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
          [RegularExpression(@"^[a-zA-Z\s-']+$", ErrorMessage = "Name can only contain letters, spaces, hyphens and apostrophes")]
          public string Username { get; set; } = string.Empty;

          [Required(ErrorMessage = "Email is required")]
          [EmailAddress(ErrorMessage = "Invalid email format")]
          [StringLength(255)]
          public string Email { get; set; } = string.Empty;

          [Required(ErrorMessage = "Password is required")]
          [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
          [DataType(DataType.Password)]
          [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
              ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character")]
          public string Password { get; set; } = string.Empty;

          [Required(ErrorMessage = "Password confirmation is required")]
          [DataType(DataType.Password)]
          [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
          public string RepeatedPassword { get; set; } = string.Empty;

          [Phone(ErrorMessage = "Invalid phone number format")]
          [StringLength(20)]
          public string? Phone { get; set; }

          public UserRole Role { get; set; } = UserRole.Customer;
     }
}
