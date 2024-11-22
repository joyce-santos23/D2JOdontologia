using SpecialtyEntity = Domain.Entities.Specialty;
using Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly ClinicaDbContext _clinicaDbContext;

        public SpecialtyRepository(ClinicaDbContext context)
        {
            _clinicaDbContext = context;
        }

        public async Task<List<SpecialtyEntity>> GetAll()
        {
            return await _clinicaDbContext.Specialty.ToListAsync();
        }

        public async Task<SpecialtyEntity> Get(int id)
        {
            return await _clinicaDbContext.Specialty.FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
