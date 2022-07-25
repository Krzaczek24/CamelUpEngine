using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.Exceptions
{
    public class NoCamelFoundException : ColourException
    {
        public NoCamelFoundException(Colour colour) : base(colour, $"No {colour.ToString().ToLower()} camel has been found") { }

        public NoCamelFoundException(Colour colour, string message) : base(colour, message) { }
    }
}
