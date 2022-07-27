using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Steps
{
    public interface IAudienceTileRemovementStep : IActionStep
    {
        public int FieldIndex { get; }
        public IAudienceTile AudienceTile { get; }
    }

    internal class AudienceTileRemovementStep : IAudienceTileRemovementStep
    {
        public int FieldIndex { get; }
        public IAudienceTile AudienceTile { get; }

        public AudienceTileRemovementStep(int fieldIndex, IAudienceTile audienceTile)
        {
            FieldIndex = fieldIndex;
            AudienceTile = audienceTile;
        }
    }
}
