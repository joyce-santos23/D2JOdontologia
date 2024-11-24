using SpecialistEntity = Domain.Entities.Specialist;
using SpecialtyEntity = Domain.Entities.Specialty;
namespace Application.Dtos
{
    public class SpecialistDto
    {
        public string Name { get; set; }
        public string Fone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string CroNumber { get; set; }
        public string CroState { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<int> SpecialtyIds { get; set; } = new List<int>();

        public static SpecialistDto MapToDto(SpecialistEntity specialist)
        {
            return new SpecialistDto
            {
                Name = specialist.Name,
                Fone = specialist.Fone,
                Address = specialist.Address,
                Email = specialist.Email,
                CroNumber = specialist.CroNumber,
                CroState = specialist.CroState,
                SpecialtyIds = specialist.Specialties.Select(s => s.Id).ToList()
            };
        }

        public static SpecialistEntity MapToEntity(SpecialistDto specialistDto)
        {
            return new SpecialistEntity
            {
                Name = specialistDto.Name,
                Fone = specialistDto.Fone,
                Address = specialistDto.Address,
                Email = specialistDto.Email,
                CroNumber = specialistDto.CroNumber,
                CroState = specialistDto.CroState,
                Specialties = specialistDto.SpecialtyIds.Select(id => new SpecialtyEntity { Id = id }).ToList()
            };
        }
    }
}
