using Domain.Entities;

namespace Domain.Ports
{
    public interface ISpecialistRepository
    {
        Task<int> Create(Domain.Entities.Specialist specialist);
        Task<Domain.Entities.Specialist> Get(int Id);
        Task<IEnumerable<Domain.Entities.Specialist>> GetAll();
        Task<bool> SpecialtyExists(int specialtyId);

    }
}
