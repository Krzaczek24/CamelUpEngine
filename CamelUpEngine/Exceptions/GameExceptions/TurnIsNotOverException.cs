namespace CamelUpEngine.Exceptions
{
    public class TurnIsNotOverException : CamelUpGameException
    {
        public TurnIsNotOverException() : base("Turn is not over yet, please perform any other action") { }
        public TurnIsNotOverException(string message) : base(message) { }
    }
}
