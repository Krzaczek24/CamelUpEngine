using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Steps
{
    public interface IChangedCurrentPlayerStep : IActionStep
    {
        public IPlayer NewPlayer { get; }
    }

    internal class ChangedCurrentPlayerStep : IChangedCurrentPlayerStep
    {
        public IPlayer NewPlayer { get; }

        public ChangedCurrentPlayerStep(IPlayer newPlayer)
        {
            NewPlayer = newPlayer;
        }
    }
}
