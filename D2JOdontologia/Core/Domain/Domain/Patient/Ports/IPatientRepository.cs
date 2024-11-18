using Domain.Entities;

namespace Domain.Ports
{
    public interface IPatientRepository
    {
        Task<Domain.Entities.Patient> Get(int Id);
        Task<IEnumerable<Domain.Entities.Patient>> GetAll();
        Task<int> Create(Domain.Entities.Patient patient);
    }
}
