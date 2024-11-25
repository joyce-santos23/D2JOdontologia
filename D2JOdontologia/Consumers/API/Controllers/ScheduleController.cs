using Application;
using Application.Dtos;
using Application.Ports;
using Application.Schedule.Requests;
using Application.Specialist.Requests;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IScheduleManager _scheduleManager;

        public ScheduleController(ILogger<ScheduleController> logger, IScheduleManager scheduleManager)
        {
            _logger = logger;
            _scheduleManager = scheduleManager;
        }

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
