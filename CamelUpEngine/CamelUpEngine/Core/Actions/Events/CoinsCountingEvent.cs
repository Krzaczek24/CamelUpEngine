using System.Collections.Generic;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface ICoinsCountingEvent : IActionSubEvents<ICoinsAddedEvent>, IActionEvent
    {

    }

    internal class CoinsCountingEvent : ActionSubEvents<ICoinsAddedEvent>, ICoinsCountingEvent
    {
        public CoinsCountingEvent(IEnumerable<ICoinsAddedEvent> subEvents) : base(subEvents)
        {
            
        }
    }
}
