using System.Collections.Generic;

namespace CamelUpEngine.Core.Enums
{
    public enum AudienceTileSide
    {
        Booing = -1,
        Cheering = 1
    }

    public static class AudienceTileSideExtension
    {
        private static IReadOnlyDictionary<AudienceTileSide, StackPutType> MapAudienceTileSideToStackPutType { get; } = new Dictionary<AudienceTileSide, StackPutType>
        {
            [AudienceTileSide.Cheering] = StackPutType.Top,
            [AudienceTileSide.Booing] = StackPutType.Bottom
        };

        public static StackPutType ToStackPutType(this AudienceTileSide audienceTileSide) => MapAudienceTileSideToStackPutType[audienceTileSide];
    }
}
