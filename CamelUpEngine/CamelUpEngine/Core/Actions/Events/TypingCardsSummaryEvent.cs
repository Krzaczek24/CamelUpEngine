using System.Collections.Generic;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface ICoinsCountingEvent : IActionSubEvents<ICoinsAddedEvent>, IActionEvent
    {

    }

    internal class TypingCardsSummaryEvent : ActionSubEvents<ICoinsAddedEvent>, ICoinsCountingEvent
    {
        public TypingCardsSummaryEvent(IEnumerable<ICoinsAddedEvent> subEvents) : base(subEvents)
        {
            
        }
    }
}
