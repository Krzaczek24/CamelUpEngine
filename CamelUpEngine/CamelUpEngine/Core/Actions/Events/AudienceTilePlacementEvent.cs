using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IAudienceTilePlacementEvent : IActionEvent
    {
        public int FieldIndex { get; }
        public IAudienceTile AudienceTile { get; }
    }

    internal class AudienceTilePlacementEvent : IAudienceTilePlacementEvent
    {
        public int FieldIndex { get; }
        public IAudienceTile AudienceTile { get; }

        public AudienceTilePlacementEvent(int fieldIndex, IAudienceTile audienceTile)
        {
            FieldIndex = fieldIndex;
            AudienceTile = audienceTile;
        }
    }
}
