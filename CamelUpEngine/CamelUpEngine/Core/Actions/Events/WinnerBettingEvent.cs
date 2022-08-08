using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IBettingEvent : IActionEvent
    {
        public IBetCard BetCard { get; }
        public BetType BetType { get; }
    }

    internal class BettingEvent : IBettingEvent
    {
        public IBetCard BetCard { get; }
        public BetType BetType { get; }

        public BettingEvent(IBetCard betCard, BetType betType)
        {
            BetCard = betCard;
            BetType = betType;
        }
    }
}
