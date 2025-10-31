using System.ComponentModel.DataAnnotations;

namespace petwell_back.Models.DTOs.Requests
{
     public class AssistanceServiceRequestDto
     {
          [Required(ErrorMessage = "Company name is required")]
          [StringLength(100, MinimumLength = 2, ErrorMessage = "Company name must be between 2 and 100 characters")]
          public required string Company { get; set; }

          [Required(ErrorMessage = "Service type is required")]
          public required string Type { get; set; }

          [Required(ErrorMessage = "Location is required")]
          [StringLength(50, ErrorMessage = "Location cannot exceed 50 characters")]
          public required string Location { get; set; }

          [Required(ErrorMessage = "Animal type is required")]
          [StringLength(50, ErrorMessage = "Animal type cannot exceed 50 characters")]
          public required string Animal { get; set; }

          [Required(ErrorMessage = "Description is required")]
          [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 2000 characters")]
          public required string Description { get; set; }

          [Required(ErrorMessage = "At least one price must be specified")]
          [MinLength(1, ErrorMessage = "At least one service price must be specified")]
          public required List<ServicePriceDto> Prices { get; set; } = new();

          [Required(ErrorMessage = "Contact information is required")]
          public required ContactsDto Contacts { get; set; } = new() { Phone = "" };

          [Required(ErrorMessage = "At least one speciality is required")]
          [MinLength(1, ErrorMessage = "At least one speciality must be specified")]
          public required List<string> Specialities { get; set; } = new();

          [Required(ErrorMessage = "Doctor and its description is required")]
          [StringLength(500, ErrorMessage = "Doctor description cannot exceed 500 characters")]
          public required string Doctor { get; set; }

          public List<CommentDto>? Comments { get; set; }

     }

     public class ServicePriceDto
     {
          [Required(ErrorMessage = "Price name is required")]
          [StringLength(100, ErrorMessage = "Price name cannot exceed 100 characters")]
          public required string Name { get; set; }

          [Required(ErrorMessage = "Price amount is required")]
          [Range(1, 100000, ErrorMessage = "Price must be between 1 and 100000")]
          public required decimal Price { get; set; }

          [Required(ErrorMessage = "Currency is required")]
          [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be a 3-letter code")]
          [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Currency must be a 3-letter uppercase code (e.g., USD, EUR)")]
          public required string Currency { get; set; }
     }

     public class ContactsDto
     {
          [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
          public string? Address { get; set; }

          [Required(ErrorMessage = "Phone number is required")]
          [StringLength(20, MinimumLength = 5, ErrorMessage = "Phone number must be between 5 and 20 characters")]
          public required string Phone { get; set; }

          [EmailAddress(ErrorMessage = "Invalid email address format")]
          [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
          public string? Email { get; set; }

          [StringLength(200, ErrorMessage = "Website URL cannot exceed 200 characters")]
          public string? Website { get; set; }
     }

     public class CommentDto
     {
          [Required(ErrorMessage = "Comment author is required")]
          [StringLength(100, ErrorMessage = "Author name cannot exceed 100 characters")]
          public required string Author { get; set; }

          [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
          public required int Rating { get; set; }

          [StringLength(1000, ErrorMessage = "Comment text must max 1000 characters")]
          public string? Content { get; set; } = string.Empty;
     }
}
