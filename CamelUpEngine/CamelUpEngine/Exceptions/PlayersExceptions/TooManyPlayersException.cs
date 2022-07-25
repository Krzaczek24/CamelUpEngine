namespace CamelUpEngine.Exceptions
{
    public class TooManyPlayersException : WrongPlayersCountException
    {
        public static int MaximalPlayersCount => Game.MaximalPlayersCount;

        public TooManyPlayersException(int actualPlayersCount) : base(actualPlayersCount, $"Too many players, maximal players count is {MaximalPlayersCount}") { }

        public TooManyPlayersException(int actualPlayersCount, string message) : base(actualPlayersCount, message) { }
    }
}
