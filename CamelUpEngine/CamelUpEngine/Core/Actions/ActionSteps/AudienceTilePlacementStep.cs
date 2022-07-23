using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.ActionSteps
{
    public interface IAudienceTilePlacementStep : IActionStep
    {
        public IField Field { get; }
    }

    //internal class AudienceTilePlacementStep : ActionStep, IAudienceTilePlacementStep
    internal class AudienceTilePlacementStep : IAudienceTilePlacementStep
    {
        public IField Field { get; }

        public AudienceTilePlacementStep(IField field)
        {
            Field = field;
        }
    }
}
