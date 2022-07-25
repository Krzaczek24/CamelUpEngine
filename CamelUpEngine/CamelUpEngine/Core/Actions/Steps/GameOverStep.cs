using CamelUpEngine.GameObjects;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Core.Actions.Steps
{
    public interface IGameOverStep : IActionStep
    {
        public IReadOnlyCollection<IPlayer> Winners { get; }
    }

    internal class GameOverStep : IGameOverStep
    {
        public IReadOnlyCollection<IPlayer> Winners { get; }

        public GameOverStep(Game game)
        {
            Winners = game.Players.OrderByDescending(player => player.Coins).ToList();
        }
    }
}
