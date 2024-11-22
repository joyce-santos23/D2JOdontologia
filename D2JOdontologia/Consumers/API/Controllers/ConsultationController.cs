using Application.Consultation.Dtos;
using Application.Dtos;
using Application.Ports;
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
        public async Task<IActionResult> CreateConsultation([FromBody] ConsultationRequestDto consultationDto)
        {
            var response = await _consultationManager.CreateConsultation(consultationDto);

            if (response.Success)
                return CreatedAtAction(nameof(GetConsultation), new { id = response.ConsultationData.Id }, response.ConsultationData);

            _logger.LogError("Failed to create consultation: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return response.ErrorCode switch
            {
                Application.ErrorCode.PATIENT_NOT_FOUND => BadRequest(new { Message = response.Message, ErrorCode = response.ErrorCode }),
                Application.ErrorCode.SCHEDULE_NOT_FOUND => BadRequest(new { Message = response.Message, ErrorCode = response.ErrorCode }),
                Application.ErrorCode.INVALID_DATE => BadRequest(new { Message = response.Message, ErrorCode = response.ErrorCode }),
                _ => StatusCode(500, new { Message = "An unexpected error occurred.", ErrorCode = response.ErrorCode })
            };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConsultation(int id)
        {
            var response = await _consultationManager.GetConsultation(id);

            if (response.Success)
                return Ok(response.ConsultationData);

            _logger.LogError("Failed to retrieve consultation: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return response.ErrorCode switch
            {
                Application.ErrorCode.CONSULTATION_NOT_FOUND => NotFound(new { Message = response.Message, ErrorCode = response.ErrorCode }),
                _ => StatusCode(500, new { Message = "An unexpected error occurred.", ErrorCode = response.ErrorCode })
            };
        }

        [HttpGet("by-date/{date}")]
        public async Task<IActionResult> GetConsultationsByDate(DateTime date)
        {
            var response = await _consultationManager.GetConsultationsByDate(date);

            if (response.Success)
                return Ok(response.Data);

            _logger.LogError("Failed to retrieve consultations by date: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return response.ErrorCode switch
            {
                Application.ErrorCode.CONSULTATION_NOT_FOUND => NotFound(new { Message = response.Message, ErrorCode = response.ErrorCode }),
                _ => StatusCode(500, new { Message = "An unexpected error occurred.", ErrorCode = response.ErrorCode })
            };
        }

        [HttpGet("by-patient/{patientId}")]
        public async Task<IActionResult> GetConsultationsByPatient(int patientId)
        {
            var response = await _consultationManager.GetConsultationsByPatient(patientId);

            if (response.Success)
                return Ok(response.Data);

            _logger.LogError("Failed to retrieve consultations by patient: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return response.ErrorCode switch
            {
                Application.ErrorCode.CONSULTATION_NOT_FOUND => NotFound(new { Message = response.Message, ErrorCode = response.ErrorCode }),
                _ => StatusCode(500, new { Message = "An unexpected error occurred.", ErrorCode = response.ErrorCode })
            };
        }

        [HttpGet("by-specialist/{specialistId}")]
        public async Task<IActionResult> GetConsultationsBySpecialist(int specialistId)
        {
            var response = await _consultationManager.GetConsultationsBySpecialist(specialistId);

            if (response.Success)
                return Ok(response.Data);

            _logger.LogError("Failed to retrieve consultations by specialist: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return response.ErrorCode switch
            {
                Application.ErrorCode.CONSULTATION_NOT_FOUND => NotFound(new { Message = response.Message, ErrorCode = response.ErrorCode }),
                _ => StatusCode(500, new { Message = "An unexpected error occurred.", ErrorCode = response.ErrorCode })
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsultation(int id, [FromBody] ConsultationUpdateRequestDto consultationDto)
        {
            var response = await _consultationManager.UpdateConsultation(id, consultationDto);

            if (response.Success)
                return Ok(response.ConsultationData);

            _logger.LogError("Failed to update consultation: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return response.ErrorCode switch
            {
                Application.ErrorCode.CONSULTATION_NOT_FOUND => NotFound(new { Message = response.Message, ErrorCode = response.ErrorCode }),
                _ => StatusCode(500, new { Message = "An unexpected error occurred.", ErrorCode = response.ErrorCode })
            };
        }

    }
}
