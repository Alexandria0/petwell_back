using petwell_back.Shared.Common;
using System.ComponentModel.DataAnnotations;

namespace petwell_back.Models.DTOs.Requests
{
     public class ChooseProfileRequestDto
     {
          [Required(ErrorMessage = "Choosing a profile is required")]
          public required UserRole Role { get; set; }
     }
}
