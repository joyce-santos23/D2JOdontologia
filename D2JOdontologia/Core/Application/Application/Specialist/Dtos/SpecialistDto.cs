
using Domain.Entities;

namespace Application.Dtos
{
    public class SpecialistDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Fone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int SpecialtyId { get; set; }
        public DateTime CreatedAt { get; set; }



        public static SpecialistDto MapToDto(Domain.Entities.Specialist specialist)
        {
            return new SpecialistDto
            {
                Id = specialist.Id,
                Name = specialist.Name,
                Fone = specialist.Fone,
                Address = specialist.Address,
                Email = specialist.Email,
                SpecialtyId = specialist.SpecialtyId,
                CreatedAt = specialist.CreatedAt,

            };
        }

        public static Specialist MapToEntity(SpecialistDto specialistDto)
        {
            return new Specialist
            {
                Id = specialistDto.Id,
                Name = specialistDto.Name,
                Fone = specialistDto.Fone,
                Address = specialistDto.Address,
                Email = specialistDto.Email,
                SpecialtyId = specialistDto.SpecialtyId,
                CreatedAt = specialistDto.CreatedAt,

            };
        }
    }
}
