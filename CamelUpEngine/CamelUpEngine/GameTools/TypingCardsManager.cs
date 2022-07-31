using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions;
using CamelUpEngine.GameObjects;
using CamelUpEngine.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.GameTools
{
    public class TypingCardsManager
    {
        private readonly static IReadOnlyCollection<TypingCardValue> initialValues = new[] { TypingCardValue.Low, TypingCardValue.Low, TypingCardValue.Medium, TypingCardValue.High };

        private Dictionary<Colour, Stack<TypingCard>> availableCards = new();
        private Func<Guid> GenerateGuid { get; }
        internal Guid DrawGuid { get; private set; }

        private IReadOnlyCollection<IAvailableTypingCard> availableCardsCache;
        public IReadOnlyCollection<IAvailableTypingCard> AvailableCards => availableCardsCache ??= availableCards.Select(stack => stack.Value.TryPeek(out TypingCard card) ? new AvailableTypingCard(card, DrawGuid) : null).Where(card => card != null).ToList();

        public TypingCardsManager(Func<Guid> guidGenerationFunction = null)
        {
            GenerateGuid = guidGenerationFunction ?? Guid.NewGuid;

            foreach (Colour colour in ColourHelper.AllCardColours)
            {
                availableCards.Add(colour, new());
            }

            Reset();
        }

        public void Reset()
        {
            foreach (Colour colour in ColourHelper.AllCardColours)
            {
                availableCards[colour].Clear();
                foreach (TypingCardValue value in initialValues)
                {
                    availableCards[colour].Push(new(colour, value));
                }
            }

            availableCardsCache = null;
            DrawGuid = GenerateGuid();
        }

        public ITypingCard DrawCard(IAvailableTypingCard availableTypingCard)
        {
            if (availableTypingCard.DrawGuid != DrawGuid)
            {
                throw new TypingCardExpiredAvailabilityException();
            }

            if (availableCards.TryGetValue(availableTypingCard.Colour, out Stack<TypingCard> stack)
            && stack.TryPeek(out TypingCard card)
            && card.Value == availableTypingCard.Value)
            {
                availableCardsCache = null;
                DrawGuid = GenerateGuid();
                return stack.Pop();
            }

            throw new TypingCardUnavailableException(availableTypingCard.Colour, availableTypingCard.Value);
        }

        public static int CountCoins(IEnumerable<ICamel> camelsOrder, IEnumerable<ITypingCard> cards)
        {
            int playerCoinsEarned = 0;
            var camelsColourOrder = camelsOrder.Where(camel => !camel.IsMad).GetColours().ToList();
            
            foreach (ITypingCard card in cards)
            {
                int rank = camelsColourOrder.IndexOf(card.Colour) + 1;
                switch (rank)
                {
                    case 1: playerCoinsEarned += (int)card.Value; break;
                    case 2: playerCoinsEarned++; break;
                    default: playerCoinsEarned--; break;
                }
            }

            return playerCoinsEarned;
        }
    }
}
