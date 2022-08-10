using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IDiceDrawnEvent : IActionEvent
    {
        public IDrawnDice DrawnDice { get; }
    }

    internal class DiceDrawnEvent : IDiceDrawnEvent
    {
        public IDrawnDice DrawnDice { get; }

        public DiceDrawnEvent(IDrawnDice drawnDice)
        {
            DrawnDice = drawnDice;
        }
    }
}
