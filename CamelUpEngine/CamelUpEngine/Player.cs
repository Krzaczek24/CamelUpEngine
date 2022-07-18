namespace CamelUpEngine
{
    public interface IPlayer
    {
        public const int INITIAL_COINS_COUNT = 3;

        public string Name { get; }
        public int Coins { get; }
    }

    internal sealed class Player : IPlayer
    {
        public string Name { get; }
        public int Coins { get; }

        private Player() { }
        public Player(string name)
        {
            Name = name;
            Coins = IPlayer.INITIAL_COINS_COUNT;
        }
    }
}
