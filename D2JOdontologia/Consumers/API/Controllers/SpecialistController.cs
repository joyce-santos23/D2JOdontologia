using Application;
using Application.Dtos;
using Application.Patient.Requests;
using Application.Ports;
using Application.Specialist.Requests;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialistController : ControllerBase
    {
        private readonly ISpecialistManager _specialistManager;
        private readonly ILogger<SpecialistController> _logger;

        public SpecialistController(ISpecialistManager specialistManager, ILogger<SpecialistController> logger)
        {
            _specialistManager = specialistManager;
            _logger = logger;
        }

        [HttpPost("create")]
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

        [HttpGet("all")]
        public async Task<IActionResult> GetAllSpecialists()
        {
            var response = await _specialistManager.GetAllSpecialists();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecialist(int id)
        {
            var response = await _specialistManager.GetSpecialist(id);

            if (!response.Success)
                return MapErrorToResponse(response.ErrorCode, "Specialist not found.");

            return Ok(response.SpecialistData);
        }

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

        [HttpPut("{specialistId}/update")]
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



    }
}
