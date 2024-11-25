using Domain.Entities;

namespace Application.Dtos
{
    public class PatientDto
    {
        public string Name { get; set; }
        public string Fone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly Birth { get; set; }
        public string Cpf { get; set; }
        public DateTime CreatedAt { get; set; } 

        public static PatientDto MapToDto(Domain.Entities.Patient patient)
        {
            return new PatientDto
            {
                Name = patient.Name,
                Fone = patient.Fone,
                Address = patient.Address,
                Email = patient.Email,
                Password = patient.PasswordHash,
                Birth = patient.Birth,
                Cpf = patient.Cpf,
                CreatedAt = patient.CreatedAt
            };
        }

        public static Domain.Entities.Patient MapToEntity(PatientDto patientDto)
        {
            return new Domain.Entities.Patient
            {
                Name = patientDto.Name,
                Fone = patientDto.Fone,
                Address = patientDto.Address,
                Email = patientDto.Email,
                PasswordHash = patientDto.Password,
                Birth = patientDto.Birth,
                Cpf = patientDto.Cpf,
                CreatedAt = patientDto.CreatedAt
            };
        }
    }
}
