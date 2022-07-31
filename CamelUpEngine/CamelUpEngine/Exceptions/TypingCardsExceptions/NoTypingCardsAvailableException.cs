using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.Exceptions
{
    public class TypingCardUnavailableException : ColourException
    {
        public TypingCardValue Value { get; }

        public TypingCardUnavailableException(Colour colour, TypingCardValue value) : base(colour, $"No more {colour.ToString().ToLower()} typing cards are available")
        {
            Value = value;
        }

        public TypingCardUnavailableException(Colour colour, TypingCardValue value, string message) : base(colour, message)
        {
            Value = value;
        }
    }
}
