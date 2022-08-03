using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface ILoserBettingEvent : IActionEvent
    {
        public IBetCard BetCard { get; }
    }

    internal class LoserBettingEvent : ILoserBettingEvent
    {
        public IBetCard BetCard { get; }

        public LoserBettingEvent(IBetCard betCard)
        {
            BetCard = betCard;
        }
    }
}
