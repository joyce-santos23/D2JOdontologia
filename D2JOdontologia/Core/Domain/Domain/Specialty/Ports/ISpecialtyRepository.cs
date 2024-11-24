using SpecialtyEntity = Domain.Entities.Specialty;

namespace Domain.Ports
{
    public interface ISpecialtyRepository
    {
        Task<List<SpecialtyEntity>> GetAll();
        Task<SpecialtyEntity> Get(int id);
        Task<List<SpecialtyEntity>> GetByIds(List<int> ids);

    }
}
