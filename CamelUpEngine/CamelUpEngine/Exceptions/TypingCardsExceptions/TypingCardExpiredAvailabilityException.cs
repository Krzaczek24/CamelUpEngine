namespace CamelUpEngine.Exceptions
{
    public class TypingCardExpiredAvailabilityException : CamelUpGameException
    {
        public TypingCardExpiredAvailabilityException() : base($"Tried to draw typing card with expired availability, please get new list of available typing cards") { }
        public TypingCardExpiredAvailabilityException(string message) : base(message) { }
    }
}
