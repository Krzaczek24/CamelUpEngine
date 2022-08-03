using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.Exceptions
{
    public abstract class ColourException : CamelUpGameException
    {
        public Colour Colour { get; }

        public ColourException(Colour colour) : base()
        {
            Colour = colour;
        }

        public ColourException(Colour colour, string message) : base(message)
        {
            Colour = colour;
        }
    }
}
