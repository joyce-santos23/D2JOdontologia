using Domain.Ports;
using Domain.Specialist.Exceptions;

namespace Domain.Entities
{
    public class Specialist : User
    {
        public String CroNumber { get; set; }
        public String CroState { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Specialty> Specialties { get; set; } = new List<Specialty>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

        public void Validate()
        {
            base.Validate();

            if (string.IsNullOrEmpty(CroNumber))
            {
                throw new InvalidCroException("CRO number must be provided.");
            }

            if (CroNumber.Length < 4 || CroNumber.Length > 6)
            {
                throw new InvalidCroException("CRO number must have between 4 and 6 characters.");
            }

            if (string.IsNullOrEmpty(CroState))
            {
                throw new InvalidCroException("CRO state must be provided.");
            }

            if (Specialties == null || !Specialties.Any())
            {
                throw new InvalidSpecialtyException("At least one specialty must be assigned.");
            }

            
        }

        public async Task Save(ISpecialistRepository specialistRepository)
        {
            this.Validate();

            if (this.Id == 0)
            {
                this.Id = await specialistRepository.Create(this);
            }
        }
    }
}
