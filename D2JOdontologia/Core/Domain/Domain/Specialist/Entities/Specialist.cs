namespace Domain.Entities
{
    public class Specialist : User
    {
        public int SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }

    }
}
