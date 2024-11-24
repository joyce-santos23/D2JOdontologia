using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using SpecialistEntity = Domain.Entities.Specialist;

namespace Data.Specialist
{
    public class SpecialistRepository : ISpecialistRepository
    {
        private readonly ClinicaDbContext _clinicaDbContext;

        public SpecialistRepository(ClinicaDbContext clinicaDbContext)
        {
            _clinicaDbContext = clinicaDbContext;
        }

        public async Task<int> Create(SpecialistEntity specialist)
        {
            _clinicaDbContext.Specialist.Add(specialist);
            await _clinicaDbContext.SaveChangesAsync();
            return specialist.Id;
        }

        public async Task<SpecialistEntity> Get(int Id)
        {
            return await _clinicaDbContext.Specialist
                .Include(s => s.Specialties)
                .FirstOrDefaultAsync(s => s.Id == Id);
        }

        public async Task<IEnumerable<SpecialistEntity>> GetAll()
        {
            return await _clinicaDbContext.Specialist
                .Include(s => s.Specialties)
                .ToListAsync();
        }

        public async Task<bool> SpecialtyExists(int specialtyId)
        {
            return await _clinicaDbContext.Specialty.AnyAsync(r => r.Id == specialtyId);
        }

        public async Task Update(SpecialistEntity specialist)
        {
            _clinicaDbContext.Specialist.Update(specialist);
            await _clinicaDbContext.SaveChangesAsync();
        }

    }
}
