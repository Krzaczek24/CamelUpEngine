using CamelUpEngine.Core;

namespace CamelUpEngine.Exceptions.CamelsExceptions
{
    public class NoCamelFoundException : ColourException
    {
        public NoCamelFoundException(Colour colour) : base($"No {colour.ToString().ToLower()} camel has been found") { }

        public NoCamelFoundException(Colour colour, string message) : base(colour, message) { }
    }
}
