using Application;
using Application.Ports;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyManager _specialtyManager;
        private readonly ILogger<SpecialtyController> _logger;

        public SpecialtyController(ISpecialtyManager specialtyManager, ILogger<SpecialtyController> logger)
        {
            _specialtyManager = specialtyManager;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllSpecialties()
        {
            var response = await _specialtyManager.GetAllSpecialties();

            if (response.Success)
                return Ok(response.Data);

            _logger.LogError("Error retrieving specialties: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecialty(int id)
        {
            var response = await _specialtyManager.GetSpecialty(id);

            if (response.Success)
                return Ok(response.SpecialtyData);

            _logger.LogError("Error retrieving specialty: {ErrorCode} - {Message}", response.ErrorCode, response.Message);

            return MapErrorToResponse(response.ErrorCode, response.Message);
        }

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
