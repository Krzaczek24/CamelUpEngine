using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IBetsSummaryEvent : IActionEvent
    {
        public IReadOnlyCollection<ICoinsAddedEvent> WinnerRewards { get; }
        public IReadOnlyCollection<ICoinsAddedEvent> LoserRewards { get; }
    }

    internal class BetsSummaryEvent : IBetsSummaryEvent
    {
        public IReadOnlyCollection<ICoinsAddedEvent> WinnerRewards { get; }
        public IReadOnlyCollection<ICoinsAddedEvent> LoserRewards { get; }

        public BetsSummaryEvent(IEnumerable<ICoinsAddedEvent> winnerRewards, IEnumerable<ICoinsAddedEvent> loserRewards) : base()
        {
            WinnerRewards = winnerRewards.ToList();
            LoserRewards = loserRewards.ToList();
        }
    }
}
