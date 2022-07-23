using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.ActionSteps
{
    public interface ICoinsAddedStep : IActionStep
    {
        public IPlayer Player { get; }
        public int CoinsCount { get; }
    }

    //internal class CoinsAddedStep : ActionStep, ICoinsAddedStep
    internal class CoinsAddedStep : ICoinsAddedStep
    {
        public IPlayer Player { get; }
        public int CoinsCount { get; }

        public CoinsAddedStep(IPlayer player, int coinsCount)
        {
            Player = player;
            CoinsCount = coinsCount;
        }
    }
}
