using Domain.User.Exceptions;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Fone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new MissingRequiredInformation();
            }

            if (string.IsNullOrEmpty(Fone))
            {
                throw new MissingRequiredInformation();
            }

            if (string.IsNullOrEmpty(Address))
            {
                throw new MissingRequiredInformation();
            }

            if (string.IsNullOrEmpty(Name))
            {
                throw new InvalidEmailException();
            }

        }




    }
}
