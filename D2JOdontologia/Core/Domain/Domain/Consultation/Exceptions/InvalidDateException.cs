

namespace Domain.Consultation.Exceptions
{
    public class InvalidDateException : Exception
    {
        public InvalidDateException(string message) : base(message) { }
    }
}
