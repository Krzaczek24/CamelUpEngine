using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IChangedCurrentPlayerEvent : IActionEvent
    {
        public IPlayer PreviousPlayer { get; }
        public IPlayer NewPlayer { get; }
    }

    internal class ChangedCurrentPlayerEvent : IChangedCurrentPlayerEvent
    {
        public IPlayer PreviousPlayer { get; }
        public IPlayer NewPlayer { get; }

        public ChangedCurrentPlayerEvent(IPlayer previousPlayer, IPlayer newPlayer)
        {
            PreviousPlayer = previousPlayer;
            NewPlayer = newPlayer;
        }
    }
}
