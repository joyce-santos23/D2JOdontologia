using Application.Dtos;
using Application.Patient.Requests;
using Application.Ports;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controlador responsável pelo gerenciamento de pacientes.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IPatientManager _patientManager;

        /// <summary>
        /// Construtor do PatientController.
        /// </summary>
        /// <param name="logger">Logger para registrar eventos e erros.</param>
        /// <param name="patientManager">Gerenciador responsável pelas operações de pacientes.</param>
        public PatientController(ILogger<PatientController> logger, IPatientManager patientManager)
        {
            _logger = logger;
            _patientManager = patientManager;
        }

        /// <summary>
        /// Cria um novo paciente.
        /// </summary>
        /// <param name="request">Dados do paciente a ser criado.</param>
        /// <returns>Detalhes do paciente criado.</returns>
        /// <response code="201">Paciente criado com sucesso.</response>
        /// <response code="400">Dados inválidos fornecidos.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="500">Erro interno ao processar a solicitação.</response>
        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePatient([FromBody] PatientDto request)
        {
            var requestObj = new CreatePatientRequest
            {
                PatientData = request
            };

            var response = await _patientManager.CreatePatient(requestObj);

            if (response.Success)
                return CreatedAtAction(nameof(GetPatient), new { id = response.PatientData.Id }, response.PatientData);

            _logger.LogError("Failed to create patient: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

        /// <summary>
        /// Obtém todos os pacientes cadastrados.
        /// </summary>
        /// <returns>Lista de pacientes.</returns>
        /// <response code="200">Retorna a lista de pacientes.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="404">Nenhum paciente encontrado.</response>
        [HttpGet("all")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetAllPatients()
        {
            var response = await _patientManager.GetAllPatient();

            if (!response.Any())
                return MapErrorToResponse(Application.ErrorCode.PATIENT_NOT_FOUND, "No patient records found.");

            return Ok(response.Select(p => p.PatientData));
        }

        /// <summary>
        /// Obtém os detalhes de um paciente específico.
        /// </summary>
        /// <param name="id">ID do paciente.</param>
        /// <returns>Detalhes do paciente.</returns>
        /// <response code="200">Paciente encontrado.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="404">Paciente não encontrado.</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetPatient(int id)
        {
            var response = await _patientManager.GetPatient(id);

            if (!response.Success)
                return MapErrorToResponse(Application.ErrorCode.PATIENT_NOT_FOUND, $"Patient with ID {id} not found.");

            return Ok(response.PatientData);
        }

        /// <summary>
        /// Atualiza os dados de um paciente.
        /// </summary>
        /// <param name="id">ID do paciente a ser atualizado.</param>
        /// <param name="updateRequest">Dados atualizados do paciente.</param>
        /// <returns>Detalhes do paciente atualizado.</returns>
        /// <response code="200">Paciente atualizado com sucesso.</response>
        /// <response code="400">Dados inválidos fornecidos.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="404">Paciente não encontrado.</response>
        /// <response code="500">Erro interno ao processar a solicitação.</response>
        [HttpPut("{id}/update")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientDto updateRequest)
        {
            var updateRequestObj = new UpdatePatientRequest
            {
                PatientData = updateRequest
            };

            var response = await _patientManager.UpdatePatient(id, updateRequestObj);

            if (response.Success)
                return Ok(response.PatientData);

            _logger.LogError("Failed to update patient: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

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
                Application.ErrorCode.MISSING_REQUIRED_INFORMATION => BadRequest(new { Message = message, ErrorCode = errorCode }),
                Application.ErrorCode.COULD_NOT_STORE_DATA => BadRequest(new { Message = message, ErrorCode = errorCode }),
                Application.ErrorCode.INVALID_EMAIL => BadRequest(new { Message = message, ErrorCode = errorCode }),
                Application.ErrorCode.PATIENT_NOT_FOUND => NotFound(new { Message = message, ErrorCode = errorCode }),
                _ => StatusCode(500, new { Message = "An unexpected error occurred.", ErrorCode = errorCode })
            };
        }
    }
}
