using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects.Core;
using System;

namespace CamelUpEngine.GameObjects
{
    public interface IAvailableTypingCard : IColourable, IValuable
    {
        internal Guid DrawGuid { get; }
    }

    internal sealed class AvailableTypingCard : TypingCardBase, IAvailableTypingCard
    {
        public Guid DrawGuid { get; }

        public AvailableTypingCard(ITypingCard typingCard, Guid drawGuid) : base(typingCard.Colour, typingCard.Value)
        {
            DrawGuid = drawGuid;
        }

        public override bool Equals(object obj)
        {
            var other = obj as AvailableTypingCard;
            if (other == null)
                return false;
            return Colour == other.Colour && Value == other.Value;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
