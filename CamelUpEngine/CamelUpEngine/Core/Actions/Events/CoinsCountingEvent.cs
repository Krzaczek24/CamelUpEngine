using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface ICoinsCountingEvent : IActionEvent, ISubEvents<ICoinsAddedEvent> { }

    internal class CoinsCountingEvent : ICoinsCountingEvent
    {
        public IReadOnlyCollection<ICoinsAddedEvent> SubEvents { get; }

        public CoinsCountingEvent(IEnumerable<ICoinsAddedEvent> subEvents)
        {
            SubEvents = subEvents.ToList();
        }
    }
}
