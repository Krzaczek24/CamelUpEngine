using System.Collections.Generic;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IAllTypingCardsReturnedEvent : IActionEvent
    {
        public IReadOnlyCollection<IPlayerTypingCardsReturnedEvent> PlayerSubEvents { get; }
    }

    internal class AllTypingCardsReturnedEvent : IAllTypingCardsReturnedEvent
    {
        public IReadOnlyCollection<IPlayerTypingCardsReturnedEvent> PlayerSubEvents { get; }

        public AllTypingCardsReturnedEvent(IReadOnlyCollection<IPlayerTypingCardsReturnedEvent> playerSubEvents)
        {
            PlayerSubEvents = playerSubEvents;
        }
    }
}
