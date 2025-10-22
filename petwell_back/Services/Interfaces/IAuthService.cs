using petwell_back.Models.DTOs.Requests;
using petwell_back.Models.DTOs.Responses;
using petwell_back.Shared.Common;

namespace petwell_back.Services.Interfaces
{
     public interface IAuthService
     {
          Task<ApiResponse?> SignUp(SignUpRequestDto request);
          Task<ApiResponse?> SignIn(SignInRequestDto request);
          Task<ApiResponse?> RefreshToken(RefreshTokenRequestDto request);
          Task<ApiResponse?> ChooseProfile(ChooseProfileRequestDto request, string userId);
     }
}
