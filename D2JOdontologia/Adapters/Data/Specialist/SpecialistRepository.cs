using Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Specialist
{
    public class SpecialistRepository : ISpecialistRepository
    {
        private readonly ClinicaDbContext _clinicaDbContext;

        public SpecialistRepository(ClinicaDbContext clinicaDbContext)
        {
            _clinicaDbContext = clinicaDbContext;
        }

        public async Task<int> Create(Domain.Entities.Specialist specialist)
        {
            _clinicaDbContext.Specialist.Add(specialist);
            await _clinicaDbContext.SaveChangesAsync();
            return specialist.Id;
        }

        public async Task<Domain.Entities.Specialist> Get(int Id)
        {
            return await _clinicaDbContext.Specialist
                .Include(s => s.Specialties)
                .FirstOrDefaultAsync(s => s.Id == Id);
        }

        public async Task<IEnumerable<Domain.Entities.Specialist>> GetAll()
        {
            return await _clinicaDbContext.Specialist
                .Include(s => s.Specialties)
                .ToListAsync();
        }

        public async Task<bool> SpecialtyExists(int specialtyId)
        {
            return await _clinicaDbContext.Specialty.AnyAsync(r => r.Id == specialtyId);
        }
    }
}
