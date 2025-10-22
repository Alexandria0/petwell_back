namespace petwell_back.Models.DTOs.Requests
{
     public class RefreshTokenRequestDto
     {
          public required string UserId { get; set; }
          public required string RefreshToken { get; set; }
     }
}
