using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.Exceptions
{
    public class NearbyFieldOccupiedByAudienceTileException : PuttingAudienceTileNotAllowedException
    {
        public override AudienceTileNotAllowedReason NotAllowedReason => AudienceTileNotAllowedReason.NearbyFieldsOccupiedByAudienceTile;        

        public NearbyFieldOccupiedByAudienceTileException(int fieldIndex) : base(fieldIndex, $"Audience tile cannot be put on {fieldIndex}. field, because nearby fields are occupied by audience tile") { }

        public NearbyFieldOccupiedByAudienceTileException(int fieldIndex, string message) : base(fieldIndex, message) { }
    }
}
