using Application;
using Application.Ports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controlador responsável pelo gerenciamento de especialidades.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyManager _specialtyManager;
        private readonly ILogger<SpecialtyController> _logger;

        /// <summary>
        /// Construtor do SpecialtyController.
        /// </summary>
        /// <param name="specialtyManager">Gerenciador de especialidades.</param>
        /// <param name="logger">Logger para registrar eventos e erros.</param>
        public SpecialtyController(ISpecialtyManager specialtyManager, ILogger<SpecialtyController> logger)
        {
            _specialtyManager = specialtyManager;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todas as especialidades cadastradas.
        /// </summary>
        /// <returns>Lista de especialidades.</returns>
        /// <response code="200">Lista de especialidades retornada com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        /// <response code="500">Erro interno ao processar a solicitação.</response>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSpecialties()
        {
            var response = await _specialtyManager.GetAllSpecialties();

            if (response.Success)
                return Ok(response.Data);

            _logger.LogError("Error retrieving specialties: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

        /// <summary>
        /// Obtém os detalhes de uma especialidade específica pelo ID.
        /// </summary>
        /// <param name="id">ID da especialidade.</param>
        /// <returns>Detalhes da especialidade.</returns>
        /// <response code="200">Especialidade encontrada com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        /// <response code="404">Especialidade não encontrada.</response>
        /// <response code="500">Erro interno ao processar a solicitação.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecialty(int id)
        {
            var response = await _specialtyManager.GetSpecialty(id);

            if (response.Success)
                return Ok(response.SpecialtyData);

            _logger.LogError("Error retrieving specialty: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

        /// <summary>
        /// Mapeia códigos de erro para respostas HTTP apropriadas.
        /// </summary>
        /// <param name="errorCode">Código de erro.</param>
        /// <param name="message">Mensagem de erro.</param>
        /// <returns>Resposta HTTP apropriada.</returns>
        private IActionResult MapErrorToResponse(ErrorCode? errorCode, string message)
        {
            return errorCode switch
            {
                ErrorCode.SPECIALTY_NOT_FOUND => NotFound(new { Message = message, ErrorCode = errorCode }),
                ErrorCode.COULD_NOT_STORE_DATA => StatusCode(500, new { Message = message, ErrorCode = errorCode }),
                _ => BadRequest(new { Message = "An unexpected error occurred.", ErrorCode = errorCode })
            };
        }
    }
}
