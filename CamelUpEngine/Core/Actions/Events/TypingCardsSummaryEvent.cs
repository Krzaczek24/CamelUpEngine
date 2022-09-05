using System.Collections.Generic;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface ITypingCardsSummaryEvent : IActionSubEvents<ICoinsAddedEvent>, IActionEvent
    {

    }

    internal class TypingCardsSummaryEvent : ActionSubEvents<ICoinsAddedEvent>, ITypingCardsSummaryEvent
    {
        public TypingCardsSummaryEvent(IEnumerable<ICoinsAddedEvent> subEvents) : base(subEvents)
        {
            
        }
    }
}
