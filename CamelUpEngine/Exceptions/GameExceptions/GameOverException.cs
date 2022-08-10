namespace CamelUpEngine.Exceptions
{
    public class GameOverException : CamelUpGameException
    {
        public GameOverException() : base("Game is already over, no actions are available") { }
        public GameOverException(string message) : base(message) { }
    }
}
