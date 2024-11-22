namespace Domain.Schedule.Exceptions
{
    public class InvalidScheduleDatesException : Exception
    {
        public InvalidScheduleDatesException(string message) : base(message) { }
    }
}
