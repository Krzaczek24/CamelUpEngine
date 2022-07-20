using CamelUpEngine.Core.Actions.ActionSteps;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameTools;
using System;

namespace CamelUpEngine.GameObjects
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
        public int Coins { get; private set; }

        private Player() { }
        public Player(string name)
        {
            Name = name;
            Coins = IPlayer.INITIAL_COINS_COUNT;
        }

        public void AddCoins(int count)
        {
            int oldCount = Coins;
            Coins = Math.Max(Coins + count, 0);
            ActionCollector.AddAction(new CoinsAddedStep(this, Coins - oldCount));
        }

        public AudienceTile GetAudienceTile(AudienceTileSide audienceTileSide) => new(this, audienceTileSide);
    }
}
