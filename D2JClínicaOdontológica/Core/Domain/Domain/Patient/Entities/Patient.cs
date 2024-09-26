namespace Domain.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public DateOnly Birth { get; set; }
        public string Cpf { get; set; }
    }
}
