using Application.Dtos;
using ScheduleEntity = Domain.Entities.Schedule;

public class ScheduleResponseDto
{
    public int Id { get; set; }
    public int SpecialistId { get; set; }
    public string SpecialistName { get; set; } 
    public DateTime Data { get; set; }
    public bool IsAvailable { get; set; }

    public static ScheduleResponseDto MapToResponseDto(ScheduleEntity entity)
    {
        return new ScheduleResponseDto
        {
            Id = entity.Id,
            SpecialistId = entity.SpecialistId,
            SpecialistName = entity.Specialist?.Name ?? "Unknown", 
            Data = entity.Data,
            IsAvailable = entity.IsAvailable
        };
    }
}


