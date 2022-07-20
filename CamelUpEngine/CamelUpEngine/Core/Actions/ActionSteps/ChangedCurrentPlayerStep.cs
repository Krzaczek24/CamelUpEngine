using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.ActionSteps
{
    public interface IChangedCurrentPlayerStep
    {
        public IPlayer NewPlayer { get; }
    }

    internal class ChangedCurrentPlayerStep : ActionStep, IChangedCurrentPlayerStep
    {
        public IPlayer NewPlayer { get; }

        public ChangedCurrentPlayerStep(IPlayer newPlayer)
        {
            NewPlayer = newPlayer;
        }
    }
}
