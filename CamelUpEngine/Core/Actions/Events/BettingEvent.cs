using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IBettingEvent : IActionEvent
    {
        public IPlayer Player { get; }
        public IBetCard BetCard { get; }
        public BetType BetType { get; }
    }

    internal class BettingEvent : IBettingEvent
    {
        public IPlayer Player { get; }
        public IBetCard BetCard { get; }
        public BetType BetType { get; }

        public BettingEvent(IPlayer player, IBetCard betCard, BetType betType)
        {
            Player = player;
            BetCard = betCard;
            BetType = betType;
        }
    }
}
