using CamelUpEngine.GameObjects;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IGameOverEvent : IActionEvent
    {
        public ICamel FirstCamel { get; }
        public ICamel LastCamel { get; }
        public IReadOnlyCollection<IPlayer> Winners { get; }
    }

    internal class GameOverEvent : IGameOverEvent
    {
        public ICamel FirstCamel { get; }
        public ICamel LastCamel { get; }
        public IReadOnlyCollection<IPlayer> Winners { get; }

        public GameOverEvent(Game game)
        {
            FirstCamel = game.Camels.First();
            LastCamel = game.Camels.Last();
            Winners = game.Players.OrderByDescending(player => player.Coins).ToList();
        }
    }
}
