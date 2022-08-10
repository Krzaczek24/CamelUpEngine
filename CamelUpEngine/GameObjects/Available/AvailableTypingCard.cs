using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects.Core;
using System;

namespace CamelUpEngine.GameObjects.Available
{
    public interface IAvailableTypingCard : IColourable, IValuable, IAvailable
    {
        
    }

    internal sealed class AvailableTypingCard : TypingCardBase, IAvailableTypingCard
    {
        public Guid DrawGuid { get; }

        public AvailableTypingCard(ITypingCard typingCard, Guid drawGuid) : base(typingCard.Colour, typingCard.Value)
        {
            DrawGuid = drawGuid;
        }

        public override string ToString() => $"{Colour} typing card with value of {(int)Value}";
    }
}
