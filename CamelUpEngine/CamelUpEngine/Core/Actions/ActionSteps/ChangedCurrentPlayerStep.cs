using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.ActionSteps
{
    public interface IChangedCurrentPlayerStep : IActionStep
    {
        public IPlayer NewPlayer { get; }
    }

    //internal class ChangedCurrentPlayerStep : ActionStep, IChangedCurrentPlayerStep
    internal class ChangedCurrentPlayerStep : IChangedCurrentPlayerStep
    {
        public IPlayer NewPlayer { get; }

        public ChangedCurrentPlayerStep(IPlayer newPlayer)
        {
            NewPlayer = newPlayer;
        }
    }
}
