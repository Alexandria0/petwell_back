using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using petwell_back.Data;
using petwell_back.Services.Implementations;
using petwell_back.Services.Interfaces;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<MongoDbService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAssistanceService, AssistanceService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(options =>
{
     options.TokenValidationParameters = new TokenValidationParameters
     {
          ValidateIssuer = true,
          ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
          ValidateAudience = true,
          ValidAudience = builder.Configuration["JwtConfig:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Token"]!)),
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true
     };
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
     options.AddPolicy("AllowNextJS", policy =>
     {
          policy
             .WithOrigins(
                 "http://localhost:3000"
             )
             .AllowAnyMethod()
             .AllowAnyHeader()
             .AllowCredentials();
     });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
     app.UseDeveloperExceptionPage();
     app.MapOpenApi();
     app.MapScalarApiReference();
}

app.UseCors("AllowNextJS");

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
