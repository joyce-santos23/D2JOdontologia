namespace Domain.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public int SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }
        public DateTime Data { get; set; }
        public Boolean IsFree { get; set; }

        public ICollection<Consultation> Consultations { get; set; }

    }
}
