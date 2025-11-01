using MongoDB.Driver;
using petwell_back.Data;
using petwell_back.Models.DTOs.Requests;
using petwell_back.Models.Entities;
using petwell_back.Services.Interfaces;
using petwell_back.Shared.Common;

namespace petwell_back.Services.Implementations
{
     public class AssistanceService(MongoDbService mongoDbService) : IAssistanceService
     {
          private readonly IMongoCollection<ServiceDb>? dbCollection = mongoDbService.Database?.GetCollection<ServiceDb>("Services");
          public async Task<ApiResponse?> CreateMedicalAssistanceService(AssistanceServiceRequestDto request, string userId)
          {
               if (dbCollection == null) return null;
               var service = new ServiceDb
               {
                    UserId = userId,
                    Company = request.Company,
                    Type = request.Type,
                    Location = request.Location,
                    Animal = request.Animal,
                    Description = request.Description,
                    Prices = request.Prices.Select(p => new ServicePrice
                    {
                         Name = p.Name,
                         Price = p.Price,
                         Currency = p.Currency
                    }).ToList(),
                    Contacts = new Contacts
                    {
                         Address = request.Contacts.Address,
                         Phone = request.Contacts.Phone,
                         Email = request.Contacts.Email,
                         Website = request.Contacts.Website
                    },
                    Specialities = request.Specialities.Select(s => new Speciality
                    {
                         Name = s.Name
                    }).ToList(),
                    Doctor = request.Doctor,
                    Comments = request.Comments?.Select(c => new Comment
                    {
                         UserId = c.Author,
                         Rating = c.Rating,
                         Content = c.Content,
                    }).ToList(),
               };
               await dbCollection.InsertOneAsync(service);
               var response = new ApiResponse
               {
                    Success = true,
                    Message = "Service created successfully.",
                    Route = "/"
               };
               return response;
          }

          public async Task<ApiResponse?> GetAssistanceServices()
          {
               if (dbCollection == null) return null;

               var services = await dbCollection.Find(_ => true).ToListAsync();

               return new ApiResponse
               {
                    Success = true,
                    Message = "Services retrieved successfully",
                    Data = services
               };
          }
     }
}