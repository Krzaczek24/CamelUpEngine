using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.Exceptions.AudienceTilesExceptions
{
    public class FieldAlreadyOccupiedByAudienceTileExcception : PuttingAudienceTileNotAllowedException
    {
        public override AudienceTileNotAllowedReason NotAllowedReason => AudienceTileNotAllowedReason.FieldAlreadyOccupiedByAudienceTile;

        public FieldAlreadyOccupiedByAudienceTileExcception(int fieldIndex) : base(fieldIndex, $"Audience tile cannot be put on {fieldIndex}. field, because it is already occupied by another audience tile") { }

        public FieldAlreadyOccupiedByAudienceTileExcception(int fieldIndex, string message) : base(fieldIndex, message) { }
    }
}
