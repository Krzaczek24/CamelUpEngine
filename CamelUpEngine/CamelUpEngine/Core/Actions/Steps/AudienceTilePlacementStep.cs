using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Steps
{
    public interface IAudienceTilePlacementStep : IActionStep
    {
        public IField Field { get; }
    }

    internal class AudienceTilePlacementStep : IAudienceTilePlacementStep
    {
        public IField Field { get; }

        public AudienceTilePlacementStep(IField field)
        {
            Field = field;
        }
    }
}
