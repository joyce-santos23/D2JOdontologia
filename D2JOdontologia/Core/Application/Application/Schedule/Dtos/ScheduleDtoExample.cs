using Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

public class ScheduleDtoExample : IExamplesProvider<ScheduleRequestDto>
{
    public ScheduleRequestDto GetExamples()
    {
        return new ScheduleRequestDto
        {

            SpecialistId = 0,
            IsAvailable = true,
            StartDate = new DateTime(2024, 11, 21),
            EndDate = new DateTime(2024, 11, 25),
            StartTime = TimeSpan.Parse("08:00:00"), 
            EndTime = TimeSpan.Parse("17:00:00"),
            IntervalMinutes = 60
        };
    }
}
