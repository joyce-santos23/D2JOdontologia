namespace Domain.Entities
{
    public class Specialty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<Specialist> Specialists { get; set; } = new List<Specialist>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
