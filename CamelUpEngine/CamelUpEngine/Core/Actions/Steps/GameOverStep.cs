using CamelUpEngine.GameObjects;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Core.Actions.Steps
{
    public interface IGameOverStep : IActionStep
    {
        public IReadOnlyCollection<IPlayer> Players { get; }
    }

    internal class GameOverStep : IGameOverStep
    {
        public IReadOnlyCollection<IPlayer> Players { get; }

        public GameOverStep(Game game)
        {
            Players = game.Players.OrderByDescending(player => player.Coins).ToList();
        }
    }
}
