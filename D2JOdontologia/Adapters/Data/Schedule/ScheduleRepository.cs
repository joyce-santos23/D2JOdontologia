using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using ScheduleEntity = Domain.Entities.Schedule;

namespace Data.Schedule
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly ClinicaDbContext _clinicaDbContext;

        public ScheduleRepository(ClinicaDbContext clinicaDbContext)
        {
            _clinicaDbContext = clinicaDbContext;
        }

        public async Task AddSchedulesAsync(IEnumerable<ScheduleEntity> schedules)
        {
            await _clinicaDbContext.Schedule.AddRangeAsync(schedules);
            await _clinicaDbContext.SaveChangesAsync();
        }

        public async Task<ScheduleEntity> GetByDateAndSpecialist(DateTime date, int specialistId)
        {
            return await _clinicaDbContext.Schedule
                .FirstOrDefaultAsync(s => s.Data == date && s.SpecialistId == specialistId);
        }

        public async Task<ScheduleEntity> Get(int scheduleId)
        {
            return await _clinicaDbContext.Schedule
                .Include(s => s.Specialist)
                .FirstOrDefaultAsync(s => s.Id == scheduleId);
        }

        public async Task<IEnumerable<ScheduleEntity>> GetAll()
        {
            return await _clinicaDbContext.Schedule.Include(s => s.Specialist).ToListAsync();
        }

        public async Task<IEnumerable<ScheduleEntity>> GetByDate(DateTime date)
        {
            return await _clinicaDbContext.Schedule
                .Where(s => s.Data.Date == date.Date)
                .Include(s => s.Specialist)
                .ToListAsync();
        }

        public async Task<IEnumerable<ScheduleEntity>> GetAvailableSchedulesBySpecialist(int specialistId)
        {
            return await _clinicaDbContext.Schedule
                .Where(s => s.SpecialistId == specialistId && s.IsAvailable)
                .Include(s => s.Specialist)
                .OrderBy(s => s.Data)
                .ToListAsync();
        }


        public async Task UpdateAsync(ScheduleEntity schedule)
        {
            _clinicaDbContext.Schedule.Update(schedule);
            await _clinicaDbContext.SaveChangesAsync();
        }
    }
}
