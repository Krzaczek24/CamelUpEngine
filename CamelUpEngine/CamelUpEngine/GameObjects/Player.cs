using CamelUpEngine.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private readonly List<TypingCard> typingCards = new();

        private Player() { }
        public Player(string name)
        {
            Name = name;
        }

        public int AddCoins(int count)
        {
            int oldCount = Coins;
            Coins = Math.Max(Coins + count, 0);
            return Coins - oldCount;
        }

        public void AddTypingCard(TypingCard typingCard) => typingCards.Add(typingCard);

        public IReadOnlyCollection<TypingCard> ReturnTypingCards()
        {
            var typingCardsCopy = typingCards.ToList();
            typingCards.Clear();
            return typingCardsCopy;
        }

        public override string ToString() => $"Player '{Name}'";
    }
}
