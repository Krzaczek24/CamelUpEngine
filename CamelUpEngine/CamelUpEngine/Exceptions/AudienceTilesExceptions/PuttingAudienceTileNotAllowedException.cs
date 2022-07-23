using CamelUpEngine.Core.Actions;
using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.Exceptions.AudienceTilesExceptions
{
    public abstract class PuttingAudienceTileNotAllowedException : CamelUpGameException//, IActionStep
    {
        public int FieldIndex { get; }
        public abstract AudienceTileNotAllowedReason NotAllowedReason { get; }

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
