using Application.Consultation.Dtos;
using Application.Dtos;
using Application.Ports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultationController : ControllerBase
    {
        private readonly IConsultationManager _consultationManager;
        private readonly ILogger<ConsultationController> _logger;

        public ConsultationController(IConsultationManager consultationManager, ILogger<ConsultationController> logger)
        {
            _consultationManager = consultationManager;
            _logger = logger;
        }

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

        [HttpGet("all")]
        [Authorize(Roles = "Specialist")]
        public async Task<IActionResult> GetAllConsultations()
        {
            var response = await _consultationManager.GetAllConsultations();

            if (!response.Success)
                return MapErrorToResponse(response.ErrorCode, response.Message);

            return Ok(response.Data);
        }

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

        [HttpGet("by-patient/{patientId}")]
        [Authorize(Roles = "Specialist,Patient")]
        public async Task<IActionResult> GetConsultationsByPatient(int patientId)
        {
            var response = await _consultationManager.GetConsultationsByPatient(patientId);

            if (response.Success)
                return Ok(response.Data);

            _logger.LogError("Failed to retrieve consultations by patient: {ErrorCode} - {Message}", response.ErrorCode, response.Message);
            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

        [HttpGet("by-specialist/{specialistId}")]
        [Authorize(Roles = "Specialist,Patient")]
        public async Task<IActionResult> GetConsultationsBySpecialist(int specialistId)
        {
            var response = await _consultationManager.GetConsultationsBySpecialist(specialistId);

            if (response.Success)
                return Ok(response.Data);

            _logger.LogError("Failed to retrieve consultations by specialist: {ErrorCode} - {Message}", response.ErrorCode, response.Message);
            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

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
