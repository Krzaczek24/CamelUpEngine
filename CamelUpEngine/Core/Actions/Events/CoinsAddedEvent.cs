using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface ICoinsAddedEvent : IActionEvent
    {
        public IPlayer Player { get; }
        public int CoinsCount { get; }
    }

    internal class CoinsAddedEvent : ICoinsAddedEvent
    {
        public IPlayer Player { get; }
        public int CoinsCount { get; }

        public CoinsAddedEvent(IPlayer player, int coinsCount)
        {
            Player = player;
            CoinsCount = coinsCount;
        }
    }
}
