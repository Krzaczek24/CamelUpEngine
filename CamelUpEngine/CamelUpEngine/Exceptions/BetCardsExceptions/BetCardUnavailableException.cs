using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Exceptions
{
    public class BetCardUnavailableException : ColourException
    {
        public IPlayer Player { get; }

        public BetCardUnavailableException(IPlayer player, Colour colour) : base(colour, $"Player {player.Name} already used {colour.ToString().ToLower()} bet card")
        {
            Player = player;
        }

        public BetCardUnavailableException(IPlayer player, Colour colour, string message) : base(colour, message)
        {
            Player = player;
        }
    }
}
