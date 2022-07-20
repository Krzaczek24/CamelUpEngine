using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.ActionSteps
{
    public interface ICoinsAddedStep
    {
        public IPlayer Player { get; }
        public int CoinsCount { get; }
    }

    internal class CoinsAddedStep : ActionStep, ICoinsAddedStep
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
