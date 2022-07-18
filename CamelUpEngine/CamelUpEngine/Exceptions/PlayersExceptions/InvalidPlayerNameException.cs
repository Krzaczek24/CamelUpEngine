namespace CamelUpEngine.Exceptions.PlayersExceptions
{
    public class InvalidPlayerNameException : CamelUpGameException
    {
        public string InvalidName { get; }

        private InvalidPlayerNameException() : base() { }

        public InvalidPlayerNameException(string invalidName) : base($"'{invalidName}' is invalid player name")
        {
            InvalidName = invalidName;
        }

        public InvalidPlayerNameException(string invalidName, string message) : base(message)
        {
            InvalidName = invalidName;
        }
    }
}
