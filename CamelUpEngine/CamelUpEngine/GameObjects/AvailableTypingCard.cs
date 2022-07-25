using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects.Core;

namespace CamelUpEngine.GameObjects
{
    public interface IAvailableTypingCard : IColourable
    {
        public TypingCardValue Value { get; }
    }

    internal sealed class AvailableTypingCard : TypingCardBase, IAvailableTypingCard
    {
        public AvailableTypingCard(TypingCard typingCard) : base(typingCard.Colour, typingCard.Value) { }
    }
}
