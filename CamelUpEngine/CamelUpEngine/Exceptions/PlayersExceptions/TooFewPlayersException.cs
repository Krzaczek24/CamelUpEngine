namespace CamelUpEngine.Exceptions
{
    public class TooFewPlayersException : WrongPlayersCountException
    {
        public static int MinimalPlayersCount => Game.MINIMAL_PLAYERS_COUNT;

        public TooFewPlayersException(int actualPlayersCount) : base(actualPlayersCount, $"Too few players, minimal players count is {MinimalPlayersCount}") { }

        public TooFewPlayersException(int actualPlayersCount, string message) : base(actualPlayersCount, message) { }
    }
}
