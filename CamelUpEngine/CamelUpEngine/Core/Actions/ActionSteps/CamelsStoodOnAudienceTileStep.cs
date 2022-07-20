using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.ActionSteps
{
    public interface ICamelsStoodOnAudienceTileStep
    {
        public IAudienceTile AudienceTile { get; }
    }

    internal class CamelsStoodOnAudienceTileStep : ActionStep, ICamelsStoodOnAudienceTileStep
    {
        public IAudienceTile AudienceTile { get; }

        public CamelsStoodOnAudienceTileStep(IAudienceTile audienceTile)
        {
            AudienceTile = audienceTile;
        }
    }
}
