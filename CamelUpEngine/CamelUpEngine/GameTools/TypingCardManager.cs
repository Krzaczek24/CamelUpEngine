using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions;
using CamelUpEngine.GameObjects;
using CamelUpEngine.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.GameTools
{
    public class TypingCardManager
    {
        private readonly static IReadOnlyCollection<TypingCardValue> initialValues = new[] { TypingCardValue.Low, TypingCardValue.Low, TypingCardValue.Medium, TypingCardValue.High };
        private Dictionary<Colour, Stack<TypingCard>> availableCards = new();
        public IReadOnlyCollection<ITypingCard> AvailableCards => availableCards.Select(stack => stack.Value.TryPeek(out TypingCard card) ? card : null).Where(card => card != null).ToList();

        public TypingCardManager()
        {
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
        }

        public ITypingCard DrawCard(Colour colour)
        {
            if (availableCards.TryGetValue(colour, out Stack<TypingCard> stack)
            && stack.TryPop(out TypingCard card))
            {
                return card;
            }

            throw new NoTypingCardsAvailableException(colour);
        }
    }
}
