using Application.Consultation.Dtos;
using Application.Dtos;
using Application.Ports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controlador para gerenciamento de consultas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultationController : ControllerBase
    {
        private readonly IConsultationManager _consultationManager;
        private readonly ILogger<ConsultationController> _logger;

        /// <summary>
        /// Construtor do ConsultationController.
        /// </summary>
        /// <param name="consultationManager">Gerenciador de consultas.</param>
        /// <param name="logger">Logger para registrar eventos e erros.</param>
        public ConsultationController(IConsultationManager consultationManager, ILogger<ConsultationController> logger)
        {
            _consultationManager = consultationManager;
            _logger = logger;
        }

        /// <summary>
        /// Cria uma nova consulta.
        /// </summary>
        /// <param name="consultationDto">Dados da consulta a ser criada.</param>
        /// <returns>Detalhes da consulta criada.</returns>
        /// <response code="201">Consulta criada com sucesso.</response>
        /// <response code="400">Dados inválidos para a consulta.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        /// <response code="500">Erro interno ao processar a requisição.</response>
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CreateConsultation([FromBody] ConsultationRequestDto consultationDto)
        {
            var response = await _consultationManager.CreateConsultation(consultationDto);

            if (response.Success)
                return CreatedAtAction(nameof(GetConsultation), new { id = response.ConsultationData.Id }, response.ConsultationData);

            _logger.LogError("Failed to create consultation: {ErrorCode} - {Message}", response.ErrorCode, response.Message);
            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

        /// <summary>
        /// Obtém todas as consultas.
        /// </summary>
        /// <returns>Lista de consultas cadastradas.</returns>
        /// <response code="200">Lista de consultas retornada com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        /// <response code="500">Erro interno ao processar a requisição.</response>
        [HttpGet("all")]
        [Authorize(Roles = "Specialist")]
        public async Task<IActionResult> GetAllConsultations()
        {
            var response = await _consultationManager.GetAllConsultations();

            if (!response.Success)
                return MapErrorToResponse(response.ErrorCode, response.Message);

            return Ok(response.Data);
        }

        /// <summary>
        /// Obtém os detalhes de uma consulta específica.
        /// </summary>
        /// <param name="id">ID da consulta.</param>
        /// <returns>Detalhes da consulta.</returns>
        /// <response code="200">Consulta retornada com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        /// <response code="404">Consulta não encontrada.</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Specialist,Patient")]
        public async Task<IActionResult> GetConsultation(int id)
        {
            var response = await _consultationManager.GetConsultation(id);

            if (response.Success)
                return Ok(response.ConsultationData);

            _logger.LogError("Failed to retrieve consultation: {ErrorCode} - {Message}", response.ErrorCode, response.Message);
            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

        /// <summary>
        /// Obtém consultas por data.
        /// </summary>
        /// <param name="date">Data das consultas.</param>
        /// <returns>Lista de consultas na data especificada.</returns>
        /// <response code="200">Consultas retornadas com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        /// <response code="404">Nenhuma consulta encontrada para a data especificada.</response>
        [HttpGet("by-date/{date}")]
        [Authorize(Roles = "Specialist,Patient")]
        public async Task<IActionResult> GetConsultationsByDate(DateTime date)
        {
            var response = await _consultationManager.GetConsultationsByDate(date);

            if (response.Success)
                return Ok(response.Data);

            _logger.LogError("Failed to retrieve consultations by date: {ErrorCode} - {Message}", response.ErrorCode, response.Message);
            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

        /// <summary>
        /// Obtém consultas de um especialista específico.
        /// </summary>
        /// <param name="specialistId">ID do especialista.</param>
        /// <returns>Lista de consultas associadas ao especialista.</returns>
        /// <response code="200">Consultas retornadas com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        /// <response code="404">Nenhuma consulta encontrada para o especialista especificado.</response>
        /// <response code="500">Erro interno ao processar a requisição.</response>
        [HttpGet("by-specialist/{specialistId}")]
        [Authorize(Roles = "Specialist,Patient")]
        public async Task<IActionResult> GetConsultationsBySpecialist(int specialistId)
        {
            var response = await _consultationManager.GetConsultationsBySpecialist(specialistId);

            if (response.Success)
                return Ok(response.Data);

            _logger.LogError("Failed to retrieve consultations for specialist {SpecialistId}: {ErrorCode} - {Message}",
                specialistId, response.ErrorCode, response.Message);

            return MapErrorToResponse(response.ErrorCode, response.Message);
        }


        /// <summary>
        /// Atualiza os dados de uma consulta.
        /// </summary>
        /// <param name="id">ID da consulta.</param>
        /// <param name="consultationDto">Dados atualizados da consulta.</param>
        /// <returns>Detalhes da consulta atualizada.</returns>
        /// <response code="200">Consulta atualizada com sucesso.</response>
        /// <response code="400">Dados inválidos para atualização.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        /// <response code="404">Consulta não encontrada.</response>
        /// <response code="500">Erro interno ao processar a requisição.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> UpdateConsultation(int id, [FromBody] ConsultationUpdateRequestDto consultationDto)
        {
            var response = await _consultationManager.UpdateConsultation(id, consultationDto);

            if (response.Success)
                return Ok(response.ConsultationData);

            _logger.LogError("Failed to update consultation: {ErrorCode} - {Message}", response.ErrorCode, response.Message);
            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

        /// <summary>
        /// Mapeia os códigos de erro para respostas HTTP apropriadas.
        /// </summary>
        /// <param name="errorCode">Código de erro da aplicação.</param>
        /// <param name="message">Mensagem de erro.</param>
        /// <returns>Resposta HTTP apropriada com a mensagem de erro.</returns>
        private IActionResult MapErrorToResponse(Application.ErrorCode errorCode, string message)
        {
            return errorCode switch
            {
                Application.ErrorCode.PATIENT_NOT_FOUND => NotFound(new { Message = message, ErrorCode = errorCode }),
                Application.ErrorCode.SCHEDULE_NOT_FOUND => NotFound(new { Message = message, ErrorCode = errorCode }),
                Application.ErrorCode.CONSULTATION_NOT_FOUND => NotFound(new { Message = message, ErrorCode = errorCode }),
                Application.ErrorCode.INVALID_DATE => BadRequest(new { Message = message, ErrorCode = errorCode }),
                Application.ErrorCode.COULD_NOT_STORE_DATA => StatusCode(500, new { Message = message, ErrorCode = errorCode }),
                _ => StatusCode(500, new { Message = "An unexpected error occurred.", ErrorCode = errorCode })
            };
        }
    }
}
