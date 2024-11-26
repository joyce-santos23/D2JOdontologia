using Application;
using Application.Dtos;
using Application.Patient.Requests;
using Application.Ports;
using Application.Specialist.Requests;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controlador responsável pelo gerenciamento de especialistas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialistController : ControllerBase
    {
        private readonly ISpecialistManager _specialistManager;
        private readonly ILogger<SpecialistController> _logger;

        /// <summary>
        /// Construtor do SpecialistController.
        /// </summary>
        /// <param name="specialistManager">Gerenciador de especialistas.</param>
        /// <param name="logger">Logger para registrar eventos e erros.</param>
        public SpecialistController(ISpecialistManager specialistManager, ILogger<SpecialistController> logger)
        {
            _specialistManager = specialistManager;
            _logger = logger;
        }

        /// <summary>
        /// Cria um novo especialista.
        /// </summary>
        /// <param name="specialist">Dados do especialista a ser criado.</param>
        /// <returns>Detalhes do especialista criado.</returns>
        /// <response code="201">Especialista criado com sucesso.</response>
        /// <response code="400">Dados inválidos fornecidos.</response>
        /// <response code="500">Erro interno ao processar a solicitação.</response>
        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateSpecialist([FromBody] SpecialistDto specialist)
        {
            var request = new CreateSpecialistRequest
            {
                SpecialistData = specialist
            };

            var response = await _specialistManager.CreateSpecialist(request);

            if (response.Success)
                return CreatedAtAction(nameof(GetSpecialist), new { id = response.SpecialistData.Id }, response.SpecialistData);

            _logger.LogError("Failed to create specialist: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

        /// <summary>
        /// Obtém todos os especialistas cadastrados.
        /// </summary>
        /// <returns>Lista de especialistas.</returns>
        /// <response code="200">Lista de especialistas retornada com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        [HttpGet("all")]
        [Authorize(Roles = "Specialist")]
        public async Task<IActionResult> GetAllSpecialists()
        {
            var response = await _specialistManager.GetAllSpecialists();
            return Ok(response);
        }

        /// <summary>
        /// Obtém os detalhes de um especialista específico.
        /// </summary>
        /// <param name="id">ID do especialista.</param>
        /// <returns>Detalhes do especialista.</returns>
        /// <response code="200">Especialista encontrado com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        /// <response code="404">Especialista não encontrado.</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Specialist")]
        public async Task<IActionResult> GetSpecialist(int id)
        {
            var response = await _specialistManager.GetSpecialist(id);

            if (!response.Success)
                return MapErrorToResponse(response.ErrorCode, "Specialist not found.");

            return Ok(response.SpecialistData);
        }

        /// <summary>
        /// Atualiza os dados de um especialista.
        /// </summary>
        /// <param name="specialistId">ID do especialista.</param>
        /// <param name="updateRequestDto">Dados atualizados do especialista.</param>
        /// <returns>Detalhes do especialista atualizado.</returns>
        /// <response code="200">Especialista atualizado com sucesso.</response>
        /// <response code="400">Dados inválidos fornecidos.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        /// <response code="404">Especialista não encontrado.</response>
        /// <response code="500">Erro interno ao processar a solicitação.</response>
        [HttpPut("{specialistId}/update")]
        [Authorize(Roles = "Specialist")]
        public async Task<IActionResult> UpdateSpecialist(int specialistId, [FromBody] UpdateSpecialistDto updateRequestDto)
        {
            var updateRequest = new UpdateSpecialistRequest
            {
                SpecialistData = updateRequestDto
            };

            var response = await _specialistManager.UpdateSpecialist(specialistId, updateRequest);

            if (response.Success)
                return Ok(response.SpecialistData);

            _logger.LogError("Failed to update specialist: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

        /// <summary>
        /// Mapeia códigos de erro para respostas HTTP apropriadas.
        /// </summary>
        /// <param name="errorCode">Código de erro.</param>
        /// <param name="message">Mensagem de erro.</param>
        /// <returns>Resposta HTTP apropriada.</returns>
        private IActionResult MapErrorToResponse(ErrorCode errorCode, string message)
        {
            return errorCode switch
            {
                ErrorCode.INVALID_EMAIL => BadRequest(new { Message = message, ErrorCode = errorCode }),
                ErrorCode.MISSING_REQUIRED_INFORMATION => BadRequest(new { Message = message, ErrorCode = errorCode }),
                ErrorCode.SPECIALTY_NOT_FOUND => NotFound(new { Message = message, ErrorCode = errorCode }),
                ErrorCode.COULD_NOT_STORE_DATA => StatusCode(500, new { Message = message, ErrorCode = errorCode }),
                ErrorCode.SPECIALIST_NOT_FOUND => NotFound(new { Message = message, ErrorCode = errorCode }),
                ErrorCode.INVALID_CRO => BadRequest(new { Message = message, ErrorCode = errorCode }),
                _ => BadRequest(new { Message = "An unexpected error occurred.", ErrorCode = errorCode })
            };
        }
    }
}
