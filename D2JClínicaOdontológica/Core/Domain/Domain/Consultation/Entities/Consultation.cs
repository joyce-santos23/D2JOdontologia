namespace Domain.Entities
{
    public class Consultation
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public Schedule Schedule { get; set; }
        public Procedure Procedure { get; set; }
        public Patient Patient { get; set; }
    }
}
