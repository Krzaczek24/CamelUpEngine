using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.ActionSteps
{
    public interface IDiceDrawnStep : IActionStep
    {
        public IDrawnDice DrawnDice { get; }
    }

    //internal class DiceDrawnStep : ActionStep, IDiceDrawnStep
    internal class DiceDrawnStep : IDiceDrawnStep
    {
        public IDrawnDice DrawnDice { get; }

        public DiceDrawnStep(IDrawnDice drawnDice)
        {
            DrawnDice = drawnDice;
        }
    }
}
