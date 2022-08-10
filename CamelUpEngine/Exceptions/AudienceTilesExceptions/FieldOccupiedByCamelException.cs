using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.Exceptions
{
    public class FieldOccupiedByCamelException : PuttingAudienceTileNotAllowedException
    {
        public override AudienceTileNotAllowedReason NotAllowedReason => AudienceTileNotAllowedReason.FieldOccupiedByCamel;

        public FieldOccupiedByCamelException(int fieldIndex) : base(fieldIndex, $"Audience tile cannot be put on {fieldIndex}. field, because it is already occupied by camel") { }

        public FieldOccupiedByCamelException(int fieldIndex, string message) : base(fieldIndex, message) { }
    }
}
