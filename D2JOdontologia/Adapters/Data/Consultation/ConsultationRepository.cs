using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Ports;
using ConsultationEntity = Domain.Entities.Consultation;
using Microsoft.EntityFrameworkCore;

namespace Data.Consultation
{
    public class ConsultationRepository : IConsultationRepository
    {
        private readonly ClinicaDbContext _clinicaDbContext;

        public ConsultationRepository(ClinicaDbContext clinicaDbContext)
        {
            _clinicaDbContext = clinicaDbContext;
        }

        public async Task<int> Create(ConsultationEntity consultation)
        {
            await _clinicaDbContext.Consultation.AddAsync(consultation);
            await _clinicaDbContext.SaveChangesAsync();
            return consultation.Id; 
        }

        public async Task<ConsultationEntity> Get(int id)
        {
            return await _clinicaDbContext.Consultation
                .Include(c => c.Patient)
                .Include(c => c.Schedule)
                .ThenInclude(s => s.Specialist)
                .FirstOrDefaultAsync(c => c.Id == id); 
        }

        public async Task<IEnumerable<ConsultationEntity>> GetByDate(DateTime date)
        {
            return await _clinicaDbContext.Consultation
                .Include(c => c.Patient)
                .Include(c => c.Schedule)
                .ThenInclude(s => s.Specialist)
                .Where(c => c.Schedule.Data.Date == date.Date) 
                .ToListAsync();
        }

        public async Task<IEnumerable<ConsultationEntity>> GetByPatient(int patientId)
        {
            return await _clinicaDbContext.Consultation
                .Include(c => c.Patient)
                .Include(c => c.Schedule)
                .ThenInclude(s => s.Specialist)
                .Where(c => c.PatientId == patientId) 
                .ToListAsync();
        }

        public async Task<IEnumerable<ConsultationEntity>> GetBySpecialist(int specialistId)
        {
            return await _clinicaDbContext.Consultation
                .Include(c => c.Patient)
                .Include(c => c.Schedule)
                .ThenInclude(s => s.Specialist)
                .Where(c => c.Schedule.SpecialistId == specialistId) 
                .ToListAsync();
        }

        public async Task Update(ConsultationEntity consultation)
        {
            _clinicaDbContext.Consultation.Update(consultation);
            await _clinicaDbContext.SaveChangesAsync();
        }
    }
}
