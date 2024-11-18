using Application.Dtos;
using Application.Patient.Requests;
using Application.Ports;
using Application.Responses;
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

        [HttpPost]
        public async Task<ActionResult<PatientDto>> Post(PatientDto patient)
        {
            var request = new CreatePatientRequest
            {
                PatientData = patient
            };

            var res = await _patientManager.CreatePatient(request);

            if (res.Success)
                return CreatedAtAction(nameof(Get), new { id = res.PatientData.Id }, res.PatientData);

            _logger.LogError("Failed to create patient: {ErrorCode} - {Message}", res.ErrorCode, res.Message);

            return res.ErrorCode switch
            {
                Application.ErrorCode.MISSING_REQUIRED_INFORMATION => BadRequest(new { Message = res.Message, ErrorCode = res.ErrorCode }),
                Application.ErrorCode.COULD_NOT_STORE_DATA => BadRequest(new { Message = res.Message, ErrorCode = res.ErrorCode }),
                Application.ErrorCode.INVALID_EMAIL => BadRequest(new { Message = res.Message, ErrorCode = res.ErrorCode }),

                _ => BadRequest(new { Message = "An error occurred while creating the patient." })
            };

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> Get(int id)
        {
            var res = await _patientManager.GetPatient(id);

            if (res.Success)
                return Ok(res.PatientData);

            return NotFound(new { message = "Patient not found" });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll()
        {
            var patients = await _patientManager.GetAllPatient();

            if (!patients.Any())
            {
                return NotFound(new PatientResponse
                {
                    Success = false,
                    Message = "No rooms records were found"
                });

            }
            var patientDtos = patients.Select(g => g.PatientData).ToList();
            return Ok(patientDtos);

        }

    }
}
