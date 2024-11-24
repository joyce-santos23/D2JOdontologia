using PatientEntity = Domain.Entities.Patient;

namespace Domain.Ports
{
    public interface IPatientRepository
    {
        Task<PatientEntity> Get(int Id);
        Task<IEnumerable<PatientEntity>> GetAll();
        Task<int> Create(PatientEntity patient);
        Task Update(PatientEntity patient);
    }
}
