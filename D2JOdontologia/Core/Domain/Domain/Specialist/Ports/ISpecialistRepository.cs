using SpecialistEntity =  Domain.Entities.Specialist;

namespace Domain.Ports
{
    public interface ISpecialistRepository
    {
        Task<int> Create(SpecialistEntity specialist);
        Task<SpecialistEntity> Get(int Id);
        Task<IEnumerable<SpecialistEntity>> GetAll();
        Task<bool> SpecialtyExists(int specialtyId);
        Task Update(SpecialistEntity specialist);

    }
}
