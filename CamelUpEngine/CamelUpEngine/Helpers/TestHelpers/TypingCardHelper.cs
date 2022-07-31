﻿#if DEBUG
using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Helpers.TestHelpers
{
    public static class TypingCardHelper
    {
        public static IReadOnlyCollection<IAvailableTypingCard> CardRepository { get; } = ColourHelper.AllCardColours.SelectMany(colour => Enum.GetValues<TypingCardValue>().Select(value => new AvailableTypingCard(new TypingCard(colour, value), Guid.Empty))).ToList();
        public static IReadOnlyCollection<IAvailableTypingCard> GetCards(TypingCardValue value) => CardRepository.Where(card => card.Value == value).ToList();
        public static IReadOnlyCollection<IAvailableTypingCard> GetCards(Colour colour) => CardRepository.Where(card => card.Colour == colour).Prepend(CardRepository.GetCard(colour, TypingCardValue.Low)).ToList();
        public static Stack<IAvailableTypingCard> GetStack(Colour colour) => new(GetCards(colour).ToList());

        public static T GetCard<T>(this IEnumerable<T> source, Colour colour, TypingCardValue value) where T : IColourable, IValuable => source.First(card => card.Colour == colour && card.Value == value);
    }
}
#endif