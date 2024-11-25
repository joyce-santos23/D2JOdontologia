using Application.User.Dtos;
using Application.User.Ports;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginManager _loginManager;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILoginManager loginManager, ILogger<LoginController> logger)
        {
            _loginManager = loginManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Invalid login data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            var token = await _loginManager.Authenticate(loginDto);

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Authentication failed for email: {Email}", loginDto.Email);
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            _logger.LogInformation("User authenticated successfully: {Email}", loginDto.Email);

            return Ok(new
            {
                Message = "Authentication successful.",
                Token = token
            });
        }
    }
}
