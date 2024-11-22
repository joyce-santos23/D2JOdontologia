using Domain.Entities;
using ScheduleEntity = Domain.Entities.Schedule;
namespace Domain.Ports
{
    public interface IScheduleRepository
    {
        Task AddSchedulesAsync(IEnumerable<ScheduleEntity> schedules);
        Task<ScheduleEntity> GetByDateAndSpecialist(DateTime date, int specialistId);
        Task<ScheduleEntity> Get(int scheduleId); 
        Task UpdateAsync(ScheduleEntity schedule); 
        Task<IEnumerable<ScheduleEntity>> GetAvailableSchedulesBySpecialist(int specialistId);
        Task<IEnumerable<ScheduleEntity>> GetAll(); 
        Task<IEnumerable<ScheduleEntity>> GetByDate(DateTime date);
    }
}
