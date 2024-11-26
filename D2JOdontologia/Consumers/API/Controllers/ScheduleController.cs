using Application;
using Application.Dtos;
using Application.Ports;
using Application.Schedule.Requests;
using Application.Specialist.Requests;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace API.Controllers
{
    /// <summary>
    /// Controlador responsável pelo gerenciamento de agendamentos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IScheduleManager _scheduleManager;

        /// <summary>
        /// Construtor do ScheduleController.
        /// </summary>
        /// <param name="logger">Logger para registrar eventos e erros.</param>
        /// <param name="scheduleManager">Gerenciador de agendamentos.</param>
        public ScheduleController(ILogger<ScheduleController> logger, IScheduleManager scheduleManager)
        {
            _logger = logger;
            _scheduleManager = scheduleManager;
        }

        /// <summary>
        /// Cria novos agendamentos.
        /// </summary>
        /// <param name="schedule">Dados do agendamento.</param>
        /// <returns>Lista de agendamentos criados.</returns>
        /// <response code="200">Agendamentos criados com sucesso.</response>
        /// <response code="400">Dados de agendamento inválidos.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        /// <response code="500">Erro interno ao processar a solicitação.</response>
        [HttpPost("Create")]
        [Authorize(Roles = "Specialist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerRequestExample(typeof(ScheduleRequestDto), typeof(ScheduleDtoExample))]
        public async Task<IActionResult> CreateSchedules([FromBody] ScheduleRequestDto schedule)
        {
            var request = new CreateScheduleRequest
            {
                ScheduleData = schedule
            };

            var response = await _scheduleManager.CreateSchedules(request);

            if (response.Success)
            {
                var responseDto = response.Data.Cast<ScheduleResponseDto>().ToList();
                return Ok(new { data = responseDto });
            }

            _logger.LogError("Failed to create schedules: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

        /// <summary>
        /// Obtém todos os agendamentos.
        /// </summary>
        /// <returns>Lista de todos os agendamentos.</returns>
        /// <response code="200">Lista de agendamentos retornada com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        [HttpGet("all")]
        [Authorize(Roles = "Specialist,Patient")]
        public async Task<IActionResult> GetAllSchedules()
        {
            var response = await _scheduleManager.GetAllSchedules();

            if (response.Success)
            {
                var responseDto = response.Data.Cast<ScheduleResponseDto>().ToList();
                return Ok(new { data = responseDto });
            }

            _logger.LogError("Failed to get all schedules: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

        /// <summary>
        /// Obtém um agendamento específico pelo ID.
        /// </summary>
        /// <param name="scheduleId">ID do agendamento.</param>
        /// <returns>Detalhes do agendamento.</returns>
        /// <response code="200">Agendamento retornado com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        /// <response code="404">Agendamento não encontrado.</response>
        [HttpGet("{scheduleId}")]
        [Authorize(Roles = "Specialist,Patient")]
        public async Task<IActionResult> GetSchedule(int scheduleId)
        {
            var response = await _scheduleManager.GetSchedule(scheduleId);

            if (response.Success)
            {
                return Ok(response.ScheduleData);
            }

            _logger.LogError("Failed to get schedule: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

        /// <summary>
        /// Obtém agendamentos por data.
        /// </summary>
        /// <param name="date">Data para buscar os agendamentos.</param>
        /// <returns>Lista de agendamentos na data especificada.</returns>
        /// <response code="200">Agendamentos retornados com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        [HttpGet("byDate")]
        [Authorize(Roles = "Specialist,Patient")]
        public async Task<IActionResult> GetSchedulesByDate([FromQuery] DateTime date)
        {
            var response = await _scheduleManager.GetSchedulesByDate(date);

            if (response.Success)
            {
                var responseDto = response.Data.Cast<ScheduleResponseDto>().ToList();
                return Ok(new { data = responseDto });
            }

            _logger.LogError("Failed to get schedules by date: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

        /// <summary>
        /// Obtém agendamentos disponíveis de um especialista.
        /// </summary>
        /// <param name="specialistId">ID do especialista.</param>
        /// <returns>Lista de agendamentos disponíveis.</returns>
        /// <response code="200">Agendamentos disponíveis retornados com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        [HttpGet("available/{specialistId}")]
        [Authorize(Roles = "Specialist,Patient")]
        public async Task<IActionResult> GetAvailableSchedules(int specialistId)
        {
            var response = await _scheduleManager.GetAvailableSchedules(specialistId);

            if (response.Success)
            {
                var responseDto = response.Data.Cast<ScheduleResponseDto>().ToList();
                return Ok(new { data = responseDto });
            }

            _logger.LogError("Failed to get available schedules: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

        /// <summary>
        /// Atualiza a disponibilidade de um agendamento.
        /// </summary>
        /// <param name="scheduleId">ID do agendamento.</param>
        /// <param name="isAvailable">Disponibilidade do agendamento.</param>
        /// <returns>Detalhes do agendamento atualizado.</returns>
        /// <response code="200">Disponibilidade atualizada com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        /// <response code="404">Agendamento não encontrado.</response>
        [HttpPut("{scheduleId}/availability")]
        [Authorize(Roles = "Specialist")]
        public async Task<IActionResult> UpdateScheduleAvailability(int scheduleId, [FromBody] bool isAvailable)
        {
            var response = await _scheduleManager.UpdateScheduleAvailability(scheduleId, isAvailable);

            if (response.Success)
            {
                return Ok(response.ScheduleData);
            }

            _logger.LogError("Failed to update schedule availability: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

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
                ErrorCode.INVALID_SCHEDULE_DATES => BadRequest(new { Message = message, ErrorCode = errorCode }),
                ErrorCode.SPECIALIST_NOT_FOUND => NotFound(new { Message = message, ErrorCode = errorCode }),
                ErrorCode.SCHEDULE_NOT_FOUND => NotFound(new { Message = message, ErrorCode = errorCode }),
                ErrorCode.COULD_NOT_STORE_DATA => StatusCode(500, new { Message = message, ErrorCode = errorCode }),
                _ => BadRequest(new { Message = "An unexpected error occurred.", ErrorCode = errorCode })
            };
        }
    }
}
