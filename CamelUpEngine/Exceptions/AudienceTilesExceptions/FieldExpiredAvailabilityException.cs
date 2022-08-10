namespace CamelUpEngine.Exceptions
{
    public class FieldExpiredAvailabilityException : CamelUpGameException
    {
        public FieldExpiredAvailabilityException() : base($"Tried to put audience tile on field with expired availability, please get new list of available fields") { }
        public FieldExpiredAvailabilityException(string message) : base(message) { }
    }
}
