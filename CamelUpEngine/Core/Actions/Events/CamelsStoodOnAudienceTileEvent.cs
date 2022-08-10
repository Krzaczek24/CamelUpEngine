using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface ICamelsStoodOnAudienceTileEvent : IActionEvent
    {
        public IAudienceTile AudienceTile { get; }
    }

    internal class CamelsStoodOnAudienceTileEvent : ICamelsStoodOnAudienceTileEvent
    {
        public IAudienceTile AudienceTile { get; }

        public CamelsStoodOnAudienceTileEvent(IAudienceTile audienceTile)
        {
            AudienceTile = audienceTile;
        }
    }
}
