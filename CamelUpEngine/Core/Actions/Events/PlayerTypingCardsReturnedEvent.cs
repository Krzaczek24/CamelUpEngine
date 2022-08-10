using CamelUpEngine.GameObjects;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IPlayerTypingCardsReturnedEvent : IActionEvent
    {
        public IPlayer Player { get; }
        public IReadOnlyCollection<ITypingCard> TypingCards { get; }
    }

    internal class PlayerTypingCardsReturnedEvent : IPlayerTypingCardsReturnedEvent
    {
        public IPlayer Player { get; }
        public IReadOnlyCollection<ITypingCard> TypingCards { get; }

        public PlayerTypingCardsReturnedEvent(IPlayer player, IEnumerable<ITypingCard> typingCards)
        {
            Player = player;
            TypingCards = typingCards.ToList();
        }
    }
}
