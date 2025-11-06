using System.ComponentModel.DataAnnotations;

namespace petwell_back.Models.DTOs.Requests
{
     public class ServicesPageRequestDto
     {
          [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
          public int Page { get; set; } = 1;

          [Range(1, 50, ErrorMessage = "PageSize must be between 1 and 50")]
          public int PageSize { get; set; } = 10;

          [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters")]
          public string? SearchTerm { get; set; }

          public string? Type { get; set; }

          public string? Animal { get; set; }

          public string? Location { get; set; }
     }
}