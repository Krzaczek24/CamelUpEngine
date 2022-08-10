namespace CamelUpEngine.Exceptions
{
    public class BetCardExpiredAvailabilityExcepton : CamelUpGameException
    {
        public BetCardExpiredAvailabilityExcepton() : base($"Tried to use bet card with expired availability, please get new list of available bet cards") { }
        public BetCardExpiredAvailabilityExcepton(string message) : base(message) { }
    }
}
