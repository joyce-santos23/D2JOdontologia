using Domain.User.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Fone { get; set; }
        public string Address { get; set; }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (!IsValidEmail(value))
                {
                    throw new InvalidEmailException("The given email is invalid.");
                }

                _email = value;
            }
        }
        public string PasswordHash { get; set; } 
        public string Role { get; set; }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                throw new MissingRequiredInformationException("Password must be at least 6 characters long.");
            }

            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new MissingRequiredInformationException("Name is required.");
            }

            if (string.IsNullOrWhiteSpace(Fone))
            {
                throw new MissingRequiredInformationException("Phone (Fone) is required.");
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                throw new MissingRequiredInformationException("Address is required.");
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                throw new InvalidEmailException("The given email is invalid.");
            }
        }
    }
}
