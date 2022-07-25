using CamelUpEngine.Core.Actions.Steps;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameTools;
using System;
using System.Collections.Generic;

namespace CamelUpEngine.GameObjects
{
    public interface IPlayer
    {
        public const int InitialCoinsCount = 3;

        public string Name { get; }
        public int Coins { get; }
        public IReadOnlyCollection<ITypingCard> TypingCards { get; }
    }

    internal sealed class Player : IPlayer
    {
        public string Name { get; }
        public int Coins { get; private set; } = IPlayer.InitialCoinsCount;
        public  IReadOnlyCollection<ITypingCard> TypingCards => typingCards;

        private readonly List<ITypingCard> typingCards = new();

        private Player() { }
        public Player(string name)
        {
            Name = name;
        }

        public void AddCoins(int count)
        {
            int oldCount = Coins;
            Coins = Math.Max(Coins + count, 0);
            ActionCollector.AddAction(new CoinsAddedStep(this, Coins - oldCount));
        }

        public void AddTypingCard(ITypingCard typingCard)
        {
            typingCards.Add(typingCard);
            ActionCollector.AddAction(new TypingCardDrawnStep(this, typingCard));
        }

        public AudienceTile GetAudienceTile(AudienceTileSide audienceTileSide) => new(this, audienceTileSide);

        public override string ToString() => $"Player '{Name}'";
    }
}
