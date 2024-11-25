using Application.Dtos;
using Application.Patient.Requests;
using Application.Ports;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IPatientManager _patientManager;

        public PatientController(ILogger<PatientController> logger, IPatientManager patientManager)
        {
            _logger = logger;
            _patientManager = patientManager;
        }

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

        [HttpGet("all")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetAllPatients()
        {
            var response = await _patientManager.GetAllPatient();

            if (!response.Any())
                return MapErrorToResponse(Application.ErrorCode.PATIENT_NOT_FOUND, "No patient records found.");

            return Ok(response.Select(p => p.PatientData));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetPatient(int id)
        {
            var response = await _patientManager.GetPatient(id);

            if (!response.Success)
                return MapErrorToResponse(Application.ErrorCode.PATIENT_NOT_FOUND, $"Patient with ID {id} not found.");

            return Ok(response.PatientData);
        }

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
