using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.Exceptions.CamelsExceptions
{
    public class NoCamelOnFieldFoundException : NoCamelFoundException
    {
        public int FieldIndex { get; }

        public NoCamelOnFieldFoundException(Colour colour, int fieldIndex) : base(colour, $"There is no {colour.ToString().ToLower()} camel on {fieldIndex}. field")
        {
            FieldIndex = fieldIndex;
        }

        public NoCamelOnFieldFoundException(Colour colour, int fieldIndex, string message) : base(colour, message)
        {
            FieldIndex = fieldIndex;
        }
    }
}
