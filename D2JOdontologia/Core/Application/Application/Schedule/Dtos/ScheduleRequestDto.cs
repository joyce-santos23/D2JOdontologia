using ScheduleEntity = Domain.Entities.Schedule;

namespace Application.Dtos
{
    public class ScheduleRequestDto
    {
       
        public int SpecialistId { get; set; }
        public bool? IsAvailable { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }  
        public TimeSpan? StartTime { get; set; } 
        public TimeSpan? EndTime { get; set; }   
        public int? IntervalMinutes { get; set; }

        public static ScheduleRequestDto MapToDto(ScheduleEntity entity)
        {
            return new ScheduleRequestDto
            {
                SpecialistId = entity.SpecialistId,
                IsAvailable = entity.IsAvailable,
                StartDate = entity.Data.Date,
                StartTime = entity.Data.TimeOfDay
            };
        }

        public static ScheduleEntity MapToEntity(ScheduleRequestDto dto)
        {
            return new ScheduleEntity
            {
                SpecialistId = dto.SpecialistId,
                Data = dto.StartDate.Value.Date + dto.StartTime.Value,
                IsAvailable = dto.IsAvailable ?? true
            };
        }
    }
}
