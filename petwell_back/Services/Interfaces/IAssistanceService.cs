using petwell_back.Data;
using petwell_back.Models.DTOs.Requests;
using petwell_back.Shared.Common;

namespace petwell_back.Services.Interfaces
{
     public interface IAssistanceService
     {
          Task<ApiResponse?> CreateMedicalAssistanceService(AssistanceServiceRequestDto request, string userId);
     }
}
