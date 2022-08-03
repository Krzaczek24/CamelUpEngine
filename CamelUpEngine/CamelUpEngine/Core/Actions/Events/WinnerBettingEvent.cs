using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IWinnerBettingEvent : IActionEvent
    {
        public IBetCard BetCard { get; }
    }

    internal class WinnerBettingEvent : IWinnerBettingEvent
    {
        public IBetCard BetCard { get; }

        public WinnerBettingEvent(IBetCard betCard)
        {
            BetCard = betCard;
        }
    }
}
