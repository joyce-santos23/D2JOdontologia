namespace Domain.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public int SpecialistId { get; set; }
        public Specialist Specialist { get; set; }
        public DateTime Data { get; set; }
        public Boolean IsAvailable { get; set; }

        public ICollection<Consultation> Consultations { get; set; }

        public bool IsFree()
        {
            return IsAvailable;
        }

        public void MarkAsReserved()
        {
            if (!IsAvailable)
                throw new InvalidOperationException("This schedule is already reserved.");

            IsAvailable = false;
        }

        public void MarkAsAvailable()
        {
            IsAvailable = true;
        }
    }
}
