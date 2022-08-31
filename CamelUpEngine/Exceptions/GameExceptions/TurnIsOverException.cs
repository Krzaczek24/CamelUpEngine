namespace CamelUpEngine.Exceptions
{
    public class TurnIsOverException : CamelUpGameException
    {
        public TurnIsOverException() : base("Turn is already over, please start new turn before any new action") { }
        public TurnIsOverException(string message) : base(message) { }
    }
}
