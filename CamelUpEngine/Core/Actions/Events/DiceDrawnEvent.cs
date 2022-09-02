using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IDiceDrawnEvent : IActionEvent
    {
        public IPlayer Player { get; }
        public IDrawnDice DrawnDice { get; }
    }

    internal class DiceDrawnEvent : IDiceDrawnEvent
    {
        public IPlayer Player { get; }
        public IDrawnDice DrawnDice { get; }

        public DiceDrawnEvent(IPlayer player, IDrawnDice drawnDice)
        {
            Player = player;
            DrawnDice = drawnDice;
        }
    }
}
