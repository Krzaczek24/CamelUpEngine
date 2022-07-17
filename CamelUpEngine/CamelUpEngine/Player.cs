namespace CamelUpEngine
{
    public interface IPlayer
    {
        public string Name { get; }
        public int Coins { get; }
    }

    public sealed class Player: IPlayer
    {
        public string Name { get; }
        public int Coins { get; }

        private Player() { }
        public Player(string name)
        {
            Name = name;
            Coins = 3;
        }
    }
}
