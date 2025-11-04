using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using petwell_back.Models.DTOs.Requests;
using petwell_back.Services.Interfaces;
using petwell_back.Shared.Common;
using System.Security.Claims;

namespace petwell_back.Controllers
{
     [Route("api/[controller]")]
     [ApiController]
     public class ServicesController(IAssistanceService assistanceService) : ControllerBase
     {
          [Authorize(Roles = "Business")]
          [HttpPost("create-service")]
          public async Task<ActionResult<ApiResponse>> CreateService(AssistanceServiceRequestDto request)
          {
               var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

               if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new ApiResponse
                    {
                         Success = false,
                         Message = "You are not authorized.",
                    });

               var response = await assistanceService.CreateMedicalAssistanceService(request, userId);

               if (response == null)
                    return BadRequest(new ApiResponse
                    {
                         Success = false,
                         Message = "Failed to create service.",
                    });

               return Ok(response);
          }

          [HttpGet("get-services")]
          public async Task<ActionResult<ApiResponse>> GetServices()
          {
               var response = await assistanceService.GetAssistanceServices();

               if (response == null)
                    return BadRequest(new ApiResponse
                    {
                         Success = false,
                         Message = "Failed to retrieve services.",
                    });

               return Ok(response);
          }

          [Authorize(Roles = "Business")]
          [HttpGet("my-services")]
          public async Task<ActionResult<ApiResponse>> GetMyServices()
          {
               var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

               if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new ApiResponse
                    {
                         Success = false,
                         Message = "You are not authorized.",
                    });

               var response = await assistanceService.GetMyAssistanceServices(userId);

               if (response == null)
                    return BadRequest(new ApiResponse
                    {
                         Success = false,
                         Message = "Failed to retrieve your services.",
                    });
               return Ok(response);
          }

          [HttpGet("get-service/{serviceId}")]
          public async Task<ActionResult<ApiResponse>> GetServiceById(string serviceId)
          {
               var response = await assistanceService.GetAssistanceServiceById(serviceId);
               if (response == null)
                    return BadRequest(new ApiResponse
                    {
                         Success = false,
                         Message = "Failed to retrieve service.",
                    });
               return Ok(response);
          }

          [Authorize(Roles = "Business")]
          [HttpDelete("remove-service/{serviceId}")]
          public async Task<ActionResult<ApiResponse>> RemoveService(string serviceId)
          {
               var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

               if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new ApiResponse
                    {
                         Success = false,
                         Message = "You are not authorized.",
                    });

               var response = await assistanceService.RemoveAssistanceService(userId, serviceId);

               if (response == null)
                    return BadRequest(new ApiResponse
                    {
                         Success = false,
                         Message = "Failed to remove service.",
                    });
               return Ok(response);
          }
     }
}
