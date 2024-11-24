public class SpecialistResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Fone { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string CroNumber { get; set; }
    public string CroState { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<SpecialtyResponseDto> Specialties { get; set; }

    public static SpecialistResponseDto MapToResponseDto(Domain.Entities.Specialist specialist)
    {
        return new SpecialistResponseDto
        {
            Id = specialist.Id,
            Name = specialist.Name,
            Fone = specialist.Fone,
            Address = specialist.Address,
            Email = specialist.Email,
            CroNumber = specialist.CroNumber,
            CroState = specialist.CroState,
            CreatedAt = specialist.CreatedAt,
            Specialties = specialist.Specialties?.Select(s => SpecialtyResponseDto.MapToResponseDto(s)).ToList()
        };
    }
}

public class SpecialtyResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public static SpecialtyResponseDto MapToResponseDto(Domain.Entities.Specialty specialty)
    {
        return new SpecialtyResponseDto
        {
            Id = specialty.Id,
            Name = specialty.Name
        };
    }
}
