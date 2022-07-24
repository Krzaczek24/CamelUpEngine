namespace CamelUpEngine.Exceptions
{
    public abstract class WrongPlayersCountException : CamelUpGameException
    {
        public int ActualPlayersCount { get; }

        private WrongPlayersCountException() : base() { }

        public WrongPlayersCountException(int actualPlayersCount, string message) : base(message)
        {
            ActualPlayersCount = actualPlayersCount;
        }
    }
}
