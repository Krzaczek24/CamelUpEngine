using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface ITypingCardDrawnEvent : IActionEvent
    {
        public IPlayer Player { get; }
        public ITypingCard TypingCard { get; }
    }

    internal class TypingCardDrawnEvent : ITypingCardDrawnEvent
    {
        public IPlayer Player { get; }
        public ITypingCard TypingCard { get; }

        public TypingCardDrawnEvent(IPlayer player, ITypingCard typingCard)
        {
            Player = player;
            TypingCard = typingCard;
        }
    }
}
