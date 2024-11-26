using Application.Dtos;
using Application.Ports;
using Application.Responses;
using Application.Schedule.Requests;
using Application.Schedule.Responses;
using Domain.Entities;
using Domain.Ports;
using Domain.Schedule.Exceptions;
using ScheduleEntity = Domain.Entities.Schedule;

namespace Application.Schedule
{
    public class ScheduleManager : IScheduleManager
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ISpecialistRepository _specialistRepository;

        public ScheduleManager(IScheduleRepository scheduleRepository, ISpecialistRepository specialistRepository)
        {
            _scheduleRepository = scheduleRepository;
            _specialistRepository = specialistRepository;
        }

        public async Task<ScheduleListResponse> CreateSchedules(CreateScheduleRequest request)
        {
            try
            {
                var scheduleDto = request.ScheduleData;

                if (!scheduleDto.StartDate.HasValue || !scheduleDto.EndDate.HasValue)
                    throw new InvalidScheduleDatesException("StartDate or EndDate cannot be null.");

                if (scheduleDto.StartDate.Value < DateTime.UtcNow)
                    throw new InvalidScheduleDatesException("The start date cannot be earlier than the current date.");

                var specialist = await _specialistRepository.Get(scheduleDto.SpecialistId);
                if (specialist == null)
                    throw new SpecialistNotFoundException("The specialist ID provided was not found.");

                var schedules = new List<ScheduleEntity>();
                var currentDate = scheduleDto.StartDate.Value;

                while (currentDate <= scheduleDto.EndDate.Value)
                {
                    var currentStartTime = currentDate.Date + scheduleDto.StartTime.Value;

                    while (currentStartTime < currentDate.Date + scheduleDto.EndTime.Value)
                    {
                        var existingSchedule = await _scheduleRepository.GetByDateAndSpecialist(currentStartTime, scheduleDto.SpecialistId);
                        if (existingSchedule != null)
                        {
                            currentStartTime = currentStartTime.AddMinutes(scheduleDto.IntervalMinutes.Value);
                            continue;
                        }

                        schedules.Add(new ScheduleEntity
                        {
                            SpecialistId = scheduleDto.SpecialistId,
                            Data = currentStartTime,
                            IsAvailable = true
                        });

                        currentStartTime = currentStartTime.AddMinutes(scheduleDto.IntervalMinutes.Value);
                    }

                    currentDate = currentDate.AddDays(1);
                }

                await _scheduleRepository.AddSchedulesAsync(schedules);

                var createdSchedules = schedules.Select(ScheduleResponseDto.MapToResponseDto).ToList();
                return new ScheduleListResponse
                {
                    Data = createdSchedules,
                    Success = true,
                    Message = "Schedules created successfully."
                };
            }
            catch (InvalidScheduleDatesException ex)
            {
                return new ScheduleListResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.INVALID_SCHEDULE_DATES,
                    Message = ex.Message
                };
            }
            catch (SpecialistNotFoundException ex)
            {
                return new ScheduleListResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.SPECIALIST_NOT_FOUND,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ScheduleListResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = $"Error creating schedules: {ex.Message}"
                };
            }
        }

        public async Task<ScheduleListResponse> GetAllSchedules()
        {
            try
            {
                var schedules = await _scheduleRepository.GetAll();

                if (!schedules.Any())
                {
                    throw new ScheduleNotFoundException();
                }

                var scheduleDtos = schedules.Select(ScheduleResponseDto.MapToResponseDto).ToList();

                return new ScheduleListResponse
                {
                    Success = true,
                    Data = scheduleDtos,
                    Message = "All schedules retrieved successfully."
                };
            }
            catch (ScheduleNotFoundException)
            {
                return new ScheduleListResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.SCHEDULE_NOT_FOUND,
                    Message = "No schedules found."
                };
            }
            catch (Exception ex)
            {
                return new ScheduleListResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = $"Error retrieving all schedules: {ex.Message}"
                };
            }
        }

        public async Task<ScheduleResponse> GetSchedule(int scheduleId)
        {
            try
            {
                var schedule = await _scheduleRepository.Get(scheduleId);
                if (schedule == null)
                {
                    return new ScheduleResponse
                    {
                        Success = false,
                        ErrorCode = ErrorCode.SCHEDULE_NOT_FOUND,
                        Message = "No schedule found with the provided ID."
                    };
                }

                return new ScheduleResponse
                {
                    Success = true,
                    ScheduleData = ScheduleResponseDto.MapToResponseDto(schedule),
                    Message = "Schedule retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new ScheduleResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = $"Error retrieving schedule: {ex.Message}"
                };
            }
        }

        public async Task<ScheduleListResponse> GetSchedulesByDate(DateTime date)
        {
            try
            {
                var schedules = await _scheduleRepository.GetByDate(date);

                if (!schedules.Any())
                {
                    throw new ScheduleNotFoundException();
                }

                var scheduleDtos = schedules.Select(ScheduleResponseDto.MapToResponseDto).ToList();

                return new ScheduleListResponse
                {
                    Success = true,
                    Data = scheduleDtos,
                    Message = "Schedules for the given date retrieved successfully."
                };
            }
            catch (ScheduleNotFoundException)
            {
                return new ScheduleListResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.SCHEDULE_NOT_FOUND,
                    Message = "No schedules found for the given date."
                };
            }
            catch (Exception ex)
            {
                return new ScheduleListResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = $"Error retrieving schedules by date: {ex.Message}"
                };
            }
        }

        public async Task<ScheduleResponse> UpdateScheduleAvailability(int id, bool isAvailable)
        {
            try
            {
                var schedule = await _scheduleRepository.Get(id);
                if (schedule == null)
                {
                    throw new ScheduleNotFoundException();
                }

                schedule.IsAvailable = isAvailable;
                await _scheduleRepository.UpdateAsync(schedule);

                var responseDto = ScheduleResponseDto.MapToResponseDto(schedule);

                return new ScheduleResponse
                {
                    Success = true,
                    ScheduleData = responseDto,
                    Message = isAvailable
                        ? "Schedule marked as available."
                        : "Schedule marked as unavailable."
                };
            }
            catch (ScheduleNotFoundException)
            {
                return new ScheduleResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.SCHEDULE_NOT_FOUND,
                    Message = "Schedule not found."
                };
            }
            catch (Exception ex)
            {
                return new ScheduleResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = $"Error updating schedule: {ex.Message}"
                };
            }
        }

        public async Task<ScheduleListResponse> GetAvailableSchedules(int specialistId)
        {
            try
            {
                var schedules = await _scheduleRepository.GetAvailableSchedulesBySpecialist(specialistId);

                if (!schedules.Any())
                {
                    throw new ScheduleNotFoundException();
                }

                var scheduleDtos = schedules.Select(ScheduleResponseDto.MapToResponseDto).ToList();

                return new ScheduleListResponse
                {
                    Success = true,
                    Data = scheduleDtos,
                    Message = "Available schedules retrieved successfully."
                };
            }
            catch (ScheduleNotFoundException)
            {
                return new ScheduleListResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.SCHEDULE_NOT_FOUND,
                    Message = "No available schedules found for the given specialist."
                };
            }
            catch (Exception ex)
            {
                return new ScheduleListResponse
                {
                    Success = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = $"Error retrieving available schedules: {ex.Message}"
                };
            }
        }

    }
}
