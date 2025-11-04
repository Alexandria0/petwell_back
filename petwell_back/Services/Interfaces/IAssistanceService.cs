using petwell_back.Data;
using petwell_back.Models.DTOs.Requests;
using petwell_back.Models.Entities;
using petwell_back.Shared.Common;

namespace petwell_back.Services.Interfaces
{
     public interface IAssistanceService
     {
          Task<ApiResponse?> CreateMedicalAssistanceService(AssistanceServiceRequestDto request, string userId);
          Task<ApiResponse?> GetAssistanceServices();
          Task<ApiResponse?> GetMyAssistanceServices(string userId);
          Task<ApiResponse?> GetAssistanceServiceById(string serviceId);
          Task<ApiResponse?> RemoveAssistanceService(string userId, string serviceId);
     }
}
