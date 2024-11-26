using Application.User.Dtos;
using Application.User.Ports;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controlador responsável pela autenticação de usuários.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginManager _loginManager;
        private readonly ILogger<LoginController> _logger;

        /// <summary>
        /// Construtor do LoginController.
        /// </summary>
        /// <param name="loginManager">Gerenciador de autenticação.</param>
        /// <param name="logger">Logger para logging de eventos e erros.</param>
        public LoginController(ILoginManager loginManager, ILogger<LoginController> logger)
        {
            _loginManager = loginManager;
            _logger = logger;
        }

        /// <summary>
        /// Autentica um usuário e gera um token JWT.
        /// </summary>
        /// <param name="loginDto">Dados de login (e-mail e senha).</param>
        /// <returns>Token JWT se a autenticação for bem-sucedida.</returns>
        /// <response code="200">Autenticação bem-sucedida. Retorna o token JWT.</response>
        /// <response code="400">Dados de login inválidos.</response>
        /// <response code="401">Falha na autenticação. E-mail ou senha incorretos.</response>
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
