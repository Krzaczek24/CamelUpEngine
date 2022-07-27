using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects.Core;
using System;

namespace CamelUpEngine.GameObjects
{
    public interface IAvailableTypingCard : IColourable
    {
        public TypingCardValue Value { get; }
        internal Guid DrawGuid { get; }
    }

    internal sealed class AvailableTypingCard : TypingCardBase, IAvailableTypingCard
    {
        public Guid DrawGuid { get; }

        public AvailableTypingCard(TypingCard typingCard, Guid drawGuid) : base(typingCard.Colour, typingCard.Value)
        {
            DrawGuid = drawGuid;
        }
    }
}
