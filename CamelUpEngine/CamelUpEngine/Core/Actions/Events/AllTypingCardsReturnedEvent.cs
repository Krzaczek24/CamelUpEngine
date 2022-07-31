using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IAllTypingCardsReturnedEvent : IActionEvent, ISubEvents<IPlayerTypingCardsReturnedEvent> { }

    internal class AllTypingCardsReturnedEvent : IAllTypingCardsReturnedEvent
    {
        public IReadOnlyCollection<IPlayerTypingCardsReturnedEvent> SubEvents { get; }

        public AllTypingCardsReturnedEvent(IEnumerable<IPlayerTypingCardsReturnedEvent> subEvents)
        {
            SubEvents = subEvents.ToList();
        }
    }
}
