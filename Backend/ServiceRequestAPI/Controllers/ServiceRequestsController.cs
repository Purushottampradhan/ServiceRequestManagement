using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceRequestAPI.DTOs;
using ServiceRequestAPI.Services;
using System.Security.Claims;

namespace ServiceRequestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly IServiceRequestService _service;
        private readonly ILogger<ServiceRequestsController> _logger;

        public ServiceRequestsController(
            IServiceRequestService service,
            ILogger<ServiceRequestsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRequestDto>>> GetAllRequests()
        {
            try
            {
                var requests = await _service.GetAllRequestsAsync();
                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllRequests: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching requests");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRequestDto>> GetRequestById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Invalid request ID");

                var request = await _service.GetRequestByIdAsync(id);
                if (request == null)
                    return NotFound($"Request with ID {id} not found");

                return Ok(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetRequestById: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching the request");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ServiceRequestDto>> CreateRequest(
            [FromBody] CreateServiceRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdRequest = await _service.CreateRequestAsync(dto);
                return CreatedAtAction(nameof(GetRequestById), 
                    new { id = createdRequest.Id }, createdRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateRequest: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the request");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceRequestDto>> UpdateRequest(
            int id,
            [FromBody] UpdateServiceRequestDto dto)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Invalid request ID");

                var username = User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
                var updatedRequest = await _service.UpdateRequestAsync(id, dto, username);

                if (updatedRequest == null)
                    return NotFound($"Request with ID {id} not found");

                return Ok(updatedRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateRequest: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the request");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRequest(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Invalid request ID");

                var result = await _service.DeleteRequestAsync(id);
                if (!result)
                    return NotFound($"Request with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteRequest: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the request");
            }
        }

        [HttpGet("filter/status")]
        public async Task<ActionResult<IEnumerable<ServiceRequestDto>>> GetRequestsByStatus(
            [FromQuery] string status)
        {
            try
            {
                if (string.IsNullOrEmpty(status))
                    return BadRequest("Status parameter is required");

                var requests = await _service.GetRequestsByStatusAsync(status);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetRequestsByStatus: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching requests");
            }
        }
    }
}