using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.Exceptions.CamelsExceptions
{
    public class CamelAlreadyOnboardException : ColourException
    {
        public int FieldIndex { get; }

        public CamelAlreadyOnboardException(Colour colour, int fieldIndex) : base(colour, $"{colour} camel already onboard, on {fieldIndex}. field")
        {
            FieldIndex = fieldIndex;
        }

        public CamelAlreadyOnboardException(Colour colour, int fieldIndex, string message) : base(colour, message)
        {
            FieldIndex = fieldIndex;
        }
    }
}
