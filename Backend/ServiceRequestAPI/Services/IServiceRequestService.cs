using ServiceRequestAPI.DTOs;
using ServiceRequestAPI.Models;

namespace ServiceRequestAPI.Services
{
    public interface IServiceRequestService
    {
        Task<IEnumerable<ServiceRequestDto>> GetAllRequestsAsync();
        Task<ServiceRequestDto> GetRequestByIdAsync(int id);
        Task<ServiceRequestDto> CreateRequestAsync(CreateServiceRequestDto dto);
        Task<ServiceRequestDto> UpdateRequestAsync(int id, UpdateServiceRequestDto dto, string updatedBy);
        Task<bool> DeleteRequestAsync(int id);
        Task<IEnumerable<ServiceRequestDto>> GetRequestsByStatusAsync(string status);
    }
}