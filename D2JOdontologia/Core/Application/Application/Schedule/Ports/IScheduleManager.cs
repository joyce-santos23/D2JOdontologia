using Application.Responses;
using Application.Schedule.Requests;
using Application.Schedule.Responses;

namespace Application.Ports
{
    public interface IScheduleManager
    {
        Task<ScheduleListResponse> CreateSchedules(CreateScheduleRequest request);
        Task<ScheduleResponse> GetSchedule(int scheduleId);
        Task<ScheduleResponse> UpdateScheduleAvailability(int id, bool isAvailable);
        Task<ScheduleListResponse> GetAvailableSchedules(int specialistId);
        Task<ScheduleListResponse> GetAllSchedules(); 
        Task<ScheduleListResponse> GetSchedulesByDate(DateTime date);
    }
}
