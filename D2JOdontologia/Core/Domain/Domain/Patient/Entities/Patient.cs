using Domain.Patient.Exceptions;

namespace Domain.Entities
{
    public class Patient : User
    {
        public DateOnly Birth { get; set; }
        public string Cpf { get; set; }

        public ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

        private void Validate()
        {
            if (string.IsNullOrEmpty(Cpf))
            {
                throw new InvalidCpfException();
            }
        }
    }
}
