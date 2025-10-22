namespace petwell_back.Models.DTOs.Responses
{
     public class TokenResponseDto
     {
          public required string Token { get; set; }
          public string? RefreshToken { get; set; }
     }
}
