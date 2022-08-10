using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IAudienceTileRemovementEvent : IActionEvent
    {
        public int FieldIndex { get; }
        public IAudienceTile AudienceTile { get; }
    }

    internal class AudienceTileRemovementEvent : IAudienceTileRemovementEvent
    {
        public int FieldIndex { get; }
        public IAudienceTile AudienceTile { get; }

        public AudienceTileRemovementEvent(int fieldIndex, IAudienceTile audienceTile)
        {
            FieldIndex = fieldIndex;
            AudienceTile = audienceTile;
        }
    }
}
