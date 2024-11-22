using Domain.User.Exceptions;


namespace Domain.Entities
{
    public class Specialty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Specialist> Specialists { get; set; } = new List<Specialist>();

        public void Validate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new MissingRequiredInformationException("Specialty name is required.");
            }
        }
    }
}
