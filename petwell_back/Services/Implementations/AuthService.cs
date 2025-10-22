using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using petwell_back.Data;
using petwell_back.Models.DTOs.Requests;
using petwell_back.Models.DTOs.Responses;
using petwell_back.Models.Entities;
using petwell_back.Services.Interfaces;
using petwell_back.Shared.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace petwell_back.Services.Implementations
{
     public class AuthService(MongoDbService mongoDbService, IConfiguration configuration) : IAuthService
     {
          private readonly IMongoCollection<UserDb>? dbCollection = mongoDbService.Database?.GetCollection<UserDb>("Users");

          public async Task<ApiResponse?> SignUp(SignUpRequestDto request)
          {
               if (dbCollection == null) return null;

               var existingUser = await dbCollection.Find(u => u.Email == request.Email).FirstOrDefaultAsync();
               if (existingUser != null) return null;

               var hashedPassword = new PasswordHasher<SignUpRequestDto>().HashPassword(new SignUpRequestDto(), request.Password);

               var user = new UserDb
               {
                    Username = request.Username,
                    PasswordHash = hashedPassword,
                    Email = request.Email,
                    Phone = request.Phone,
                    Role = request.Role
               };

               await dbCollection.InsertOneAsync(user);

               var tokens = new TokenResponseDto { Token = CreateAccessToken(user), RefreshToken = await CreateRefreshToken(user) };

               var response = new ApiResponse
               {
                    Success = true,
                    Message = "Signed up successfully.",
                    Data = tokens,
                    Route = "/choose-profile"
               };

               return response;
          }

          public async Task<ApiResponse?> SignIn(SignInRequestDto request)
          {
               if (dbCollection == null) return null;

               var existingUser = await dbCollection.Find(u => u.Email == request.Email).FirstOrDefaultAsync();
               if (existingUser == null) return null;

               var result = new PasswordHasher<SignInRequestDto>().VerifyHashedPassword(new SignInRequestDto(), existingUser.PasswordHash, request.Password);
               if (result == PasswordVerificationResult.Failed) return null;

               var refreshToken = await CreateRefreshToken(existingUser);
               if (refreshToken == null) return null;

               var tokens = new TokenResponseDto { Token = CreateAccessToken(existingUser), RefreshToken = refreshToken };

               var response = new ApiResponse
               {
                    Success = true,
                    Message = "Signed in successfully.",
                    Data = tokens,
                    Route = "/"
               };

               return response;
          }

          public async Task<ApiResponse?> RefreshToken(RefreshTokenRequestDto request)
          {
               var user = await ValidateRefreshToken(request.UserId, request.RefreshToken);
               if (user == null) return null;

               var newAccessToken = CreateAccessToken(user);
               var newRefreshToken = await CreateRefreshToken(user);
               if (newRefreshToken == null) return null;

               var response = new ApiResponse
               {
                    Success = true,
                    Message = "Token refreshed successfully.",
                    Data = new TokenResponseDto { Token = newAccessToken, RefreshToken = newRefreshToken },
                    Route = "/"
               };

               return response;
          }

          private static string GenerateRefreshToken()
          {
               var randomNumber = new byte[32];
               using var rng = RandomNumberGenerator.Create();
               rng.GetBytes(randomNumber);
               return Convert.ToBase64String(randomNumber);
          }

          private string CreateAccessToken(UserDb user)
          {
               var claims = new List<Claim>
               {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
               };
               var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtConfig:Token")!));

               var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

               var tokenDescriptor = new JwtSecurityToken(
                    issuer: configuration.GetValue<string>("JwtConfig:Issuer"),
                    audience: configuration.GetValue<string>("JwtConfig:Audience"),
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: creds
                    );

               return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
          }

          private async Task<string?> CreateRefreshToken(UserDb user)
          {
               if (dbCollection == null) return null;

               var refreshToken = GenerateRefreshToken();
               user.RefreshToken = refreshToken;
               user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
               var filter = Builders<UserDb>.Filter.Eq(u => u.Id, user.Id);
               var update = Builders<UserDb>.Update
                    .Set(u => u.RefreshToken, refreshToken)
                    .Set(u => u.RefreshTokenExpiryTime, user.RefreshTokenExpiryTime);
               await dbCollection.UpdateOneAsync(filter, update);
               return refreshToken;
          }

          private async Task<UserDb?> ValidateRefreshToken(string userId, string refreshToken)
          {
               if (dbCollection == null) return null;
               var user = await dbCollection.Find(u => u.Id == userId).FirstOrDefaultAsync();
               if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow || user.RefreshToken != refreshToken) return null;
               return user;
          }

          public async Task<ApiResponse?> ChooseProfile(ChooseProfileRequestDto request, string userId)
          {
               if (dbCollection == null) return null;
               var user = await dbCollection.Find(u => u.Id == userId).FirstOrDefaultAsync();
               if (user == null) return null;
               user.Role = request.Role;
               var filter = Builders<UserDb>.Filter.Eq(u => u.Id, user.Id);
               var update = Builders<UserDb>.Update.Set(u => u.Role, user.Role);
               await dbCollection.UpdateOneAsync(filter, update);

               var newAccessToken = CreateAccessToken(user);

               var response = new ApiResponse
               {
                    Success = true,
                    Message = "Profile updated successfully.",
                    Data = new TokenResponseDto { Token = newAccessToken },
                    Route = "/"
               };
               return response;
          }
     }
}
