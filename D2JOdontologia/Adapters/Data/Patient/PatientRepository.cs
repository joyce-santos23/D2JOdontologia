using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using PatientEntity = Domain.Entities.Patient;

namespace Data.Patient
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ClinicaDbContext _clinicaDbContext;

        public PatientRepository(ClinicaDbContext clinicaDbContext)
        {
            _clinicaDbContext = clinicaDbContext;
        }

        public async Task<int> Create(PatientEntity patient)
        {
            _clinicaDbContext.Patient.Add(patient);
            await _clinicaDbContext.SaveChangesAsync();
            return patient.Id;

        }

        public async Task<PatientEntity> Get(int Id)
        {
            return await _clinicaDbContext.Patient.Where(p => p.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PatientEntity>> GetAll()
        {
            return await _clinicaDbContext.Patient.ToListAsync();
        }

        public async Task Update(PatientEntity patient)
        {
            _clinicaDbContext.Patient.Update(patient);
            await _clinicaDbContext.SaveChangesAsync();
        }

    }
}
