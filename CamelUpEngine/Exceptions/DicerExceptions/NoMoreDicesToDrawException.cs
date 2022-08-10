namespace CamelUpEngine.Exceptions
{
    public class NoMoreDicesToDrawException : CamelUpGameException
    {
        public NoMoreDicesToDrawException() : base($"There is no more dices which could be drawn") { }
        public NoMoreDicesToDrawException(string message) : base(message) { }
    }
}
