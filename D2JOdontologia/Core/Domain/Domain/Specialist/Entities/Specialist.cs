using Domain.Specialist.Exceptions;

namespace Domain.Entities
{
    public class Specialist : User
    {
        public int SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }
        public DateTime CreatedAt { get; set; }

        public void Validate()
        {
            
            if (SpecialtyId <= 0)
            {
                throw new InvalidSpecialtyException();
            }

            if (Specialty == null)
            {
                throw new InvalidSpecialtyException();
            }

            base.Validate();
        }
    }
}
