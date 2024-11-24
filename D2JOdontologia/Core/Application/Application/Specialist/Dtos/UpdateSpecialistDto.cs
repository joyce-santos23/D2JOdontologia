namespace Application.Dtos
{
    public class UpdateSpecialistDto
    {
        public string? Address { get; set; } 
        public string? Fone { get; set; } 
        public List<int>? SpecialtyIds { get; set; }
    }
}
