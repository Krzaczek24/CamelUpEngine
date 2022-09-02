using CamelUpEngine.Core.Actions.Events;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions;
using CamelUpEngine.GameObjects;
using CamelUpEngine.GameObjects.Available;
using CamelUpEngine.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.GameTools
{
    public class BetCardsManager
    {
        private readonly Dictionary<IPlayer, List<BetCard>> playersBetCard = new();
        private readonly Stack<BetCard> winnerBetsStack = new();
        private readonly Stack<BetCard> loserBetsStack = new();

        private Func<Guid> GenerateGuid { get; }
        internal Guid DrawGuid { get; private set; }

        public IReadOnlyCollection<IBetCard> WinnerBetsStack => winnerBetsStack;
        public IReadOnlyCollection<IBetCard> LoserBetsStack => loserBetsStack;

        public IReadOnlyCollection<IBetCard> MaskedWinnerBetsStack => winnerBetsStack.Select(bet => new BetCard(bet.Owner, Colour.Mad)).ToList();
        public IReadOnlyCollection<IBetCard> MaskedLoserBetsStack => loserBetsStack.Select(bet => new BetCard(bet.Owner, Colour.Mad)).ToList();

        public BetCardsManager(IEnumerable<IPlayer> players, Func<Guid> guidGenerationFunction = null)
        {
            GenerateGuid = guidGenerationFunction ?? Guid.NewGuid;

            foreach (IPlayer player in players)
            {
                playersBetCard.Add(player, ColourHelper.AllCardColours.Select(colour => new BetCard(player, colour)).ToList());
                ((Player)player).BindBetCardsFunction(GetPlayerAvailableBetCards);
            }
        }

        public IBettingEvent MakeBet(IPlayer player, IAvailableBetCard availableBetCard, BetType betType)
        {
            if (DrawGuid != availableBetCard.DrawGuid)
            {
                throw new BetCardExpiredAvailabilityExcepton();
            }

            BetCard betCard = playersBetCard[player].SingleOrDefault(card => card.Colour == availableBetCard.Colour);
            if (betCard == null)
            {
                throw new BetCardUnavailableException(player, availableBetCard.Colour);
            }

            switch (betType)
            {
                case BetType.Winner: winnerBetsStack.Push(betCard); break;
                case BetType.Loser: loserBetsStack.Push(betCard); break;
                default: throw new NotImplementedException($"Not implemented value {betType}");
            }
            playersBetCard[player].Remove(betCard);

            RegenerateGuid();
            return new BettingEvent(player, betCard, betType);
        }

        internal IReadOnlyCollection<IAvailableBetCard> GetPlayerAvailableBetCards(IPlayer player) => playersBetCard[player].Select(card => new AvailableBetCard(card.Colour, DrawGuid)).ToList();

        public static IReadOnlyDictionary<IPlayer, int> CountCoins(ICamel topCamel, IEnumerable<IBetCard> cards)
        {
            Dictionary<IPlayer, int> playersCoinsEarned = cards.Select(card => card.Owner).Distinct().ToDictionary(player => player, player => 0);
            IList<IBetCard> betCards = cards.ToList();

            for (int cardIndex = 0; cardIndex < betCards.Count(); cardIndex++)
            {
                playersCoinsEarned[betCards[cardIndex].Owner] += GetCoinsPrice(betCards, cardIndex, topCamel.Colour);
            }
            playersCoinsEarned = playersCoinsEarned.Where(entry => entry.Value != 0).ToDictionary(entry => entry.Key, entry => entry.Value);

            return playersCoinsEarned;
        }

        private static int GetCoinsPrice(IList<IBetCard> cards, int cardIndex, Colour camelColour)
        {
            if (cards[cardIndex].Colour == camelColour)
            {
                switch (cardIndex)
                {
                    case 0: return 8;
                    case 1: return 5;
                    case 2: return 3;
                    case 3: return 2;
                    default: return 1;
                }
            }

            return -1;
        }

        public void RegenerateGuid() => DrawGuid = GenerateGuid();
    }
}
