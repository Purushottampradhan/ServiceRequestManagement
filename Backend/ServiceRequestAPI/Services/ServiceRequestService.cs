using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceRequestAPI.Data;
using ServiceRequestAPI.DTOs;
using ServiceRequestAPI.Models;

namespace ServiceRequestAPI.Services
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceRequestService> _logger;

        public ServiceRequestService(
            ApplicationDbContext context,
            IMapper mapper,
            ILogger<ServiceRequestService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ServiceRequestDto>> GetAllRequestsAsync()
        {
            try
            {
                var requests = await _context.ServiceRequests
                    .OrderByDescending(r => r.CreatedDate)
                    .ToListAsync();
                return _mapper.Map<IEnumerable<ServiceRequestDto>>(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching all requests: {ex.Message}");
                throw;
            }
        }

        public async Task<ServiceRequestDto> GetRequestByIdAsync(int id)
        {
            try
            {
                var request = await _context.ServiceRequests.FindAsync(id);
                if (request == null)
                {
                    _logger.LogWarning($"Request with ID {id} not found");
                    return null;
                }
                return _mapper.Map<ServiceRequestDto>(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching request {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<ServiceRequestDto> CreateRequestAsync(CreateServiceRequestDto dto)
        {
            try
            {
                var request = new ServiceRequest
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    CreatedBy = dto.CreatedBy,
                    Status = "Open",
                    CreatedDate = DateTime.UtcNow
                };

                _context.ServiceRequests.Add(request);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Service request created with ID {request.Id}");
                return _mapper.Map<ServiceRequestDto>(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating service request: {ex.Message}");
                throw;
            }
        }

        public async Task<ServiceRequestDto> UpdateRequestAsync(int id, UpdateServiceRequestDto dto, string updatedBy)
        {
            try
            {
                var request = await _context.ServiceRequests.FindAsync(id);
                if (request == null)
                {
                    _logger.LogWarning($"Request with ID {id} not found for update");
                    return null;
                }

                if (!string.IsNullOrEmpty(dto.Title))
                    request.Title = dto.Title;

                if (!string.IsNullOrEmpty(dto.Description))
                    request.Description = dto.Description;

                if (!string.IsNullOrEmpty(dto.Status))
                    request.Status = dto.Status;

                request.UpdatedDate = DateTime.UtcNow;
                request.UpdatedBy = updatedBy;

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Service request {id} updated");
                return _mapper.Map<ServiceRequestDto>(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating service request {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteRequestAsync(int id)
        {
            try
            {
                var request = await _context.ServiceRequests.FindAsync(id);
                if (request == null)
                {
                    _logger.LogWarning($"Request with ID {id} not found for deletion");
                    return false;
                }

                _context.ServiceRequests.Remove(request);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Service request {id} deleted");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting service request {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ServiceRequestDto>> GetRequestsByStatusAsync(string status)
        {
            try
            {
                var requests = await _context.ServiceRequests
                    .Where(r => r.Status == status)
                    .OrderByDescending(r => r.CreatedDate)
                    .ToListAsync();
                return _mapper.Map<IEnumerable<ServiceRequestDto>>(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching requests by status {status}: {ex.Message}");
                throw;
            }
        }
    }
}