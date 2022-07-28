using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IChangedCurrentPlayerEvent : IActionEvent
    {
        public IPlayer NewPlayer { get; }
    }

    internal class ChangedCurrentPlayerEvent : IChangedCurrentPlayerEvent
    {
        public IPlayer NewPlayer { get; }

        public ChangedCurrentPlayerEvent(IPlayer newPlayer)
        {
            NewPlayer = newPlayer;
        }
    }
}
