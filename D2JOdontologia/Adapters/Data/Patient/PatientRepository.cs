using Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Patient
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ClinicaDbContext _clinicaDbContext;

        public PatientRepository(ClinicaDbContext clinicaDbContext)
        {
            _clinicaDbContext = clinicaDbContext;
        }

        public async Task<int> Create(Domain.Entities.Patient patient)
        {
            _clinicaDbContext.Patient.Add(patient);
            await _clinicaDbContext.SaveChangesAsync();
            return patient.Id;

        }

        public async Task<Domain.Entities.Patient> Get(int Id)
        {
            return await _clinicaDbContext.Patient.Where(p => p.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Patient>> GetAll()
        {
            return await _clinicaDbContext.Patient.ToListAsync();
        }
    }
}
