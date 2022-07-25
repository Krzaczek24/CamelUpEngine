using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.GameObjects
{
    public interface ITypingCard : IColourable
    {
        public TypingCardValue Value { get; }
    }

    internal sealed class TypingCard : ITypingCard
    {
        public Colour Colour { get; }
        public TypingCardValue Value { get; }

        public TypingCard(Colour colour, TypingCardValue value)
        {
            Colour = colour;
            Value = value;
        }

        public override string ToString() => $"{Colour} typing card with value of {(int)Value}";

        public override bool Equals(object obj)
        {
            TypingCard other = obj as TypingCard;
            if (other == null)
                return false;
            return Colour == other.Colour && Value == other.Value;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
