namespace Domain.Entities
{
    public class Procedure
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public decimal Cost { get; set; }
    }
}
