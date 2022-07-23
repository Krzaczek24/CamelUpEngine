using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.Exceptions.AudienceTilesExceptions
{
    public class PuttingAudienceTileOnStartFieldException : PuttingAudienceTileNotAllowedException
    {
        public override AudienceTileNotAllowedReason NotAllowedReason => AudienceTileNotAllowedReason.StartingFieldCannotBeOccupiedByAudienceTile;

        public PuttingAudienceTileOnStartFieldException(int fieldIndex) : base(fieldIndex, $"Audience tile cannot be put on {fieldIndex}. field, because starting field cannot be occupied by audience tile") { }

        public PuttingAudienceTileOnStartFieldException(int fieldIndex, string message) : base(fieldIndex, message) { }
    }
}
