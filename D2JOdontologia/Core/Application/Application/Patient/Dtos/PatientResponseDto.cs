using Domain.Entities;

namespace Application.Dtos
{
    public class PatientResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Fone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateOnly Birth { get; set; }
        public string Cpf { get; set; }
        public DateTime CreatedAt { get; set; }

        public static PatientResponseDto MapToResponseDto(Domain.Entities.Patient patient)
        {
            return new PatientResponseDto
            {
                Id = patient.Id,
                Name = patient.Name,
                Fone = patient.Fone,
                Address = patient.Address,
                Email = patient.Email,
                Birth = patient.Birth,
                Cpf = patient.Cpf,
                CreatedAt = patient.CreatedAt
            };
        }

    }
}
