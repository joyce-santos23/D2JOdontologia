
using ConsultationEntity = Domain.Entities.Consultation;

namespace Domain.Ports
{
    public interface IConsultationRepository
    {
        Task<int> Create(ConsultationEntity consultation);
        Task<IEnumerable<ConsultationEntity>> GetByDate(DateTime date);
        Task<IEnumerable<ConsultationEntity>> GetByPatient(int patientId);
        Task<IEnumerable<ConsultationEntity>> GetBySpecialist(int specialistId);
        Task<ConsultationEntity> Get(int id);
        Task Update(ConsultationEntity consultation);
        Task<IEnumerable<ConsultationEntity>> GetAll();

    }
}
