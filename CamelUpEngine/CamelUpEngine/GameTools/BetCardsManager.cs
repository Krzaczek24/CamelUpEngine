using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.GameTools
{
    public class BetCardsManager
    {
        public static IReadOnlyDictionary<IPlayer, int> CountCoins(ICamel camel, IEnumerable<IBetCard> cards)
        {
            Dictionary<IPlayer, int> playersCoinsEarned = cards.Select(card => card.Owner).Distinct().ToDictionary(player => player, player => 0);
            IList<IBetCard> betCards = cards.ToList();

            for (int cardIndex = 0; cardIndex < betCards.Count(); cardIndex++)
            {
                playersCoinsEarned[betCards[cardIndex].Owner] += GetCoinsPrice(betCards, cardIndex, camel.Colour);
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
    }
}
