namespace Domain.Entities
{
    public class Consultation
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int ProcedureId { get; set; }
        public Procedure Procedure { get; set; }
        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
    }
}
