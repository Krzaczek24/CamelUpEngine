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
    }
}
