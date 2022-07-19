namespace CamelUpEngine.Exceptions.AudienceTilesExceptions
{
    public class PuttingAudienceTileNotAllowedException : CamelUpGameException
    {
        public int FieldIndex { get; }

        public PuttingAudienceTileNotAllowedException(int fieldIndex) : base($"Audience tile cannot be put on {fieldIndex}. field")
        {
            FieldIndex = fieldIndex;
        }

        public PuttingAudienceTileNotAllowedException(int fieldIndex, string message) : base(message)
        {
            FieldIndex = fieldIndex;
        }
    }
}
