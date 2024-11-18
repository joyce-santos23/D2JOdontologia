namespace Domain.User.Exceptions
{
    public class MissingRequiredInformationException : Exception
    {
        public MissingRequiredInformationException(string message) : base(message) { }
    }
}
