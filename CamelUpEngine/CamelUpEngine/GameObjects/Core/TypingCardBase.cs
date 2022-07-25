using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.GameObjects.Core
{
    internal abstract class TypingCardBase : IColourable
    {
        public Colour Colour { get; }
        public TypingCardValue Value { get; }

        public TypingCardBase(Colour colour, TypingCardValue value)
        {
            Colour = colour;
            Value = value;
        }

        public override string ToString() => $"{Colour} typing card with value of {(int)Value}";

        public override bool Equals(object obj)
        {
            TypingCardBase other = obj as TypingCardBase;
            if (other == null)
                return false;
            return Colour == other.Colour && Value == other.Value;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
