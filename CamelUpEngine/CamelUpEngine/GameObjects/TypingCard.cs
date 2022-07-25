using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects.Core;

namespace CamelUpEngine.GameObjects
{
    public interface ITypingCard : IColourable
    {
        public TypingCardValue Value { get; }
    }

    internal sealed class TypingCard : TypingCardBase, ITypingCard
    {
        public TypingCard(Colour colour, TypingCardValue value) : base(colour, value) { }
    }
}
