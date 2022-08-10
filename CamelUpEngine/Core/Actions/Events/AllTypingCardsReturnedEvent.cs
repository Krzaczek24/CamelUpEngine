using System.Collections.Generic;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IAllTypingCardsReturnedEvent : IActionSubEvents<IPlayerTypingCardsReturnedEvent>, IActionEvent
    {

    }

    internal class AllTypingCardsReturnedEvent : ActionSubEvents<IPlayerTypingCardsReturnedEvent>, IAllTypingCardsReturnedEvent
    {
        public AllTypingCardsReturnedEvent(IEnumerable<IPlayerTypingCardsReturnedEvent> subEvents) : base(subEvents)
        {
            
        }
    }
}
