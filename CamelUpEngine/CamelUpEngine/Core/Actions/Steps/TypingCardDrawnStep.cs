using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Steps
{
    public interface ITypingCardDrawnStep : IActionStep
    {
        public IPlayer Player { get; }
        public ITypingCard TypingCard { get; }
    }

    internal class TypingCardDrawnStep : ITypingCardDrawnStep
    {
        public IPlayer Player { get; }
        public ITypingCard TypingCard { get; }

        public TypingCardDrawnStep(IPlayer player, ITypingCard typingCard)
        {
            Player = player;
            TypingCard = typingCard;
        }
    }
}
