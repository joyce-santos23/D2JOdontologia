using Domain.Consultation.Exceptions;
using Domain.Patient.Exceptions;
using Domain.Ports;
using Domain.Schedule.Exceptions;

namespace Domain.Entities
{
    public class Consultation
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public string Procedure { get; set; } = "Consulta";
        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public void Validate()
        {
            if (PatientId <= 0)
                throw new PatientNotFoundException();

            if (ScheduleId <= 0)
                throw new ScheduleNotFoundException();

            if (Schedule.Data < CreatedAt)
                throw new InvalidDateException("Schedule date cannot be earlier than the creation date.");
        }

        public void SetCreatedAtIfNotSet()
        {
            if (CreatedAt == default)
                CreatedAt = DateTime.UtcNow;
        }

        public async Task Save(IConsultationRepository consultationRepository)
        {
            this.Validate();

            if (this.Id == 0)
            {
                this.Id = await consultationRepository.Create(this);
            }
        }
    }
}
