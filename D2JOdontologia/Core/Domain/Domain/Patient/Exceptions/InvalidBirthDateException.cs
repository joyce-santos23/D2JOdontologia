namespace Domain.Patient.Exceptions
{
    public class InvalidBirthDateException : Exception
    {
        public InvalidBirthDateException(string message) : base(message)
        {
        }
    }
}
