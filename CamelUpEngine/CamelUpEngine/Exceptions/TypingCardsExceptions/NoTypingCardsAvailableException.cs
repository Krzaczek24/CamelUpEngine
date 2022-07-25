using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.Exceptions
{
    public class NoTypingCardsAvailableException : ColourException
    {
        public NoTypingCardsAvailableException(Colour colour) : base(colour, $"No more {colour.ToString().ToLower()} typing cards are available") { }
        public NoTypingCardsAvailableException(Colour colour, string message) : base(colour, message) { }
    }
}
