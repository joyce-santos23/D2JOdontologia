namespace Application.Dtos
{
    public class SpecialtyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static SpecialtyDto MapToDto(Domain.Entities.Specialty specialty)
        {
            return new SpecialtyDto
            {
                Id = specialty.Id,
                Name = specialty.Name
            };
        }

        public static Domain.Entities.Specialty MapToEntity(SpecialtyDto specialtyDto)
        {
            return new Domain.Entities.Specialty
            {
                Id = specialtyDto.Id,
                Name = specialtyDto.Name
            };
        }
    }
}
