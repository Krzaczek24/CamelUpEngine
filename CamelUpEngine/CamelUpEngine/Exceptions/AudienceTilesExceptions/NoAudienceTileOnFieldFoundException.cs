namespace CamelUpEngine.Exceptions
{
    public class NoAudienceTileOnFieldFoundException : CamelUpGameException
    {
        public int FieldIndex { get; }

        public NoAudienceTileOnFieldFoundException(int fieldIndex) : base($"There is no audience tile on {fieldIndex}. field")
        {
            FieldIndex = fieldIndex;
        }

        public NoAudienceTileOnFieldFoundException(int fieldIndex, string message) : base(message)
        {
            FieldIndex = fieldIndex;
        }
    }
}
