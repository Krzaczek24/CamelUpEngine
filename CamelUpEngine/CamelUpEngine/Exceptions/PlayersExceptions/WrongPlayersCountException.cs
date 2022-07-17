namespace CamelUpEngine.Exceptions.PlayersExceptions
{
    public class WrongPlayersCountException : CamelUpGameException
    {
        public int ActualPlayersCount { get; }

        private WrongPlayersCountException() : base() { }

        public WrongPlayersCountException(int actualPlayersCount, string message) : base(message)
        {
            ActualPlayersCount = actualPlayersCount;
        }
    }
}
