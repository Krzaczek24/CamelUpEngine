using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Helpers
{
    public static class TypingCardHelper
    {
        public static IReadOnlyCollection<ITypingCard> CardRepository { get; } = ColourHelper.AllCardColours.SelectMany(colour => Enum.GetValues<TypingCardValue>().Select(value => new TypingCard(colour, value))).ToList();
        public static IReadOnlyCollection<ITypingCard> GetCards(TypingCardValue value) => CardRepository.Where(card => card.Value == value).ToList();
        public static IReadOnlyCollection<ITypingCard> GetCards(Colour colour) => CardRepository.Where(card => card.Colour == colour).Prepend(GetCard(colour, TypingCardValue.Low)).ToList();
        public static Stack<ITypingCard> GetStack(Colour colour) => new(GetCards(colour).ToList());
        public static ITypingCard GetCard(Colour colour, TypingCardValue value) => CardRepository.Single(card => card.Colour == colour && card.Value == value);
    }
}
