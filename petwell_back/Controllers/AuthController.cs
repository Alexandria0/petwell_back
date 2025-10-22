using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using petwell_back.Models.DTOs.Requests;
using petwell_back.Models.DTOs.Responses;
using petwell_back.Services.Interfaces;
using petwell_back.Shared.Common;
using System.Security.Claims;

namespace petwell_back.Controllers
{
     [EnableCors("AllowNextJS")]
     [Route("api/[controller]")]
     [ApiController]
     public class AuthController(IAuthService authService) : ControllerBase
     {
          [HttpPost("sign-up")]
          public async Task<ActionResult<ApiResponse>> SignUp(SignUpRequestDto request)
          {
               try
               {
                    if (!ModelState.IsValid)
                         return BadRequest(new ApiResponse
                         {
                              Success = false,
                              Message = "Invalid sign-up data",
                              Errors = ModelState.Values
                                 .SelectMany(v => v.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList()
                         });

                    var response = await authService.SignUp(request);

                    if (response == null)
                         return Conflict(new ApiResponse
                         {
                              Success = false,
                              Message = "Something went wrong.",
                              Errors = new List<string> { "Something went wrong." }
                         });


                    return Ok(response);
               }
               catch (Exception ex)
               {
                    return StatusCode(500, new ApiResponse
                    {
                         Success = false,
                         Message = "An error occurred during sign-up",
                         Errors = new List<string> { ex.Message }
                    });
               }
          }

          [HttpPost("sign-in")]
          public async Task<ActionResult<ApiResponse>> SignIn(SignInRequestDto request)
          {
               try
               {
                    if (!ModelState.IsValid)
                         return BadRequest(new ApiResponse
                         {
                              Success = false,
                              Message = "Invalid Sign-in data",
                              Errors = ModelState.Values
                                 .SelectMany(v => v.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList()
                         });

                    var response = await authService.SignIn(request);

                    if (response == null)
                         return Conflict(new ApiResponse
                         {
                              Success = false,
                              Message = "Something went wrong.",
                              Errors = new List<string> { "Something went wrong." }
                         });

                    return Ok(response);
               }
               catch (Exception ex)
               {
                    return StatusCode(500, new ApiResponse
                    {
                         Success = false,
                         Message = "An error occurred during sign-in",
                         Errors = new List<string> { ex.Message }
                    });
               }
          }

          [HttpPost("refresh-token")]
          public async Task<ActionResult<ApiResponse>> RefreshToken(RefreshTokenRequestDto request)
          {
               var result = await authService.RefreshToken(request);
               if (result == null)
                    return Unauthorized(new ApiResponse
                    {
                         Success = false,
                         Message = "Invalid refresh token",
                         Errors = new List<string> { "Invalid refresh token" }
                    });

               return Ok(result);
          }

          [HttpPost("choose-profile")]
          public async Task<ActionResult<ApiResponse>> ChooseProfile(ChooseProfileRequestDto request)
          {
               var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

               if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new ApiResponse
                    {
                         Success = false,
                         Message = "User ID not found in token",
                         Errors = new List<string> { "User ID not found in token" }
                    });

               var result = await authService.ChooseProfile(request, userId);

               if (result == null)
                    return Unauthorized(new ApiResponse
                    {
                         Success = false,
                         Message = "Profile selection failed",
                         Errors = new List<string> { "Profile selection failed" }
                    });
               return Ok(result);
          }
          //test
     }
}