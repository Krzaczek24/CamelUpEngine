using CamelUpEngine.GameObjects;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IGameOverEvent : IActionEvent
    {
        public IReadOnlyCollection<IPlayer> Winners { get; }
    }

    internal class GameOverEvent : IGameOverEvent
    {
        public IReadOnlyCollection<IPlayer> Winners { get; }

        public GameOverEvent(Game game)
        {
            Winners = game.Players.OrderByDescending(player => player.Coins).ToList();
        }
    }
}
