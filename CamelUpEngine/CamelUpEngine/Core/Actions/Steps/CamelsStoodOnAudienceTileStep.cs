using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Steps
{
    public interface ICamelsStoodOnAudienceTileStep : IActionStep
    {
        public IAudienceTile AudienceTile { get; }
    }

    internal class CamelsStoodOnAudienceTileStep : ICamelsStoodOnAudienceTileStep
    {
        public IAudienceTile AudienceTile { get; }

        public CamelsStoodOnAudienceTileStep(IAudienceTile audienceTile)
        {
            AudienceTile = audienceTile;
        }
    }
}
