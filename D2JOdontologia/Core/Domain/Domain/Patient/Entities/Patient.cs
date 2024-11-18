using Domain.Patient.Exceptions;
using Domain.Ports;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Patient : User
    {
        public DateOnly Birth { get; set; }
        public string Cpf { get; set; }
        public DateTime CreatedAt { get;  set; }
        public ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

       
        public Patient()
        {
            CreatedAt = DateTime.UtcNow;
        }

        private void Validate()
        {
            base.Validate();

            if (string.IsNullOrWhiteSpace(Cpf) || Cpf.Length < 11)
            {
                throw new InvalidCpfException("The CPF must have at least 11 characters.");
            }

            if (Birth > DateOnly.FromDateTime(DateTime.Now))
            {
                throw new InvalidBirthDateException("The birth date can't be in the future.");
            }

            var today = DateOnly.FromDateTime(DateTime.Now);
            int monthsDifference = ((today.Year - Birth.Year) * 12) + today.Month - Birth.Month;

            if (today.Day < Birth.Day)
            {
                monthsDifference--;
            }

            if (monthsDifference < 7)
            {
                throw new InvalidBirthDateException("The patient must be at least 7 months old.");
            }
        }

        public async Task Save(IPatientRepository patientRepository)
        {
            this.Validate();

            if (this.Id == 0)
            {
                this.Id = await patientRepository.Create(this);
            }
        }
    }
}
