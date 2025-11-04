using Microsoft.AspNetCore.Mvc;
using ServiceRequestAPI.Auth;

namespace ServiceRequestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenHandler _tokenHandler;
        private readonly ILogger<AuthController> _logger;

        // Hardcoded credentials for demo - replace with database in production
        private readonly Dictionary<string, string> _validUsers = new()
        {
            { "admin", "admin123" },
            { "user", "user123" }
        };

        public AuthController(JwtTokenHandler tokenHandler, ILogger<AuthController> logger)
        {
            _tokenHandler = tokenHandler;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new AuthResponse
                    {
                        Success = false,
                        Message = "Username and password are required"
                    });
                }

                if (!_validUsers.TryGetValue(request.Username, out var password) || password != request.Password)
                {
                    _logger.LogWarning($"Failed login attempt for user: {request.Username}");
                    return Unauthorized(new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid username or password"
                    });
                }

                var token = _tokenHandler.GenerateToken(request.Username);
                _logger.LogInformation($"User {request.Username} logged in successfully");

                return Ok(new AuthResponse
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    Username = request.Username
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login error: {ex.Message}");
                return StatusCode(500, new AuthResponse
                {
                    Success = false,
                    Message = "An error occurred during login"
                });
            }
        }
    }
}