namespace CamelUpEngine.Exceptions.PlayersExceptions
{
    public class NotUniquePlayersNameException : CamelUpGameException
    {
        public NotUniquePlayersNameException() : base("All players have to got unique name") { }
        public NotUniquePlayersNameException(string message) : base(message) { }
    }
}
