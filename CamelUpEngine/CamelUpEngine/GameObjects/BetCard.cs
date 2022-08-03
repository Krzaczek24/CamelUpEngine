using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.GameObjects
{
    public interface IBetCard : IColourable
    {
        public IPlayer Owner { get; }
    }

    internal class BetCard : IBetCard
    {
        public Colour Colour { get; }
        public IPlayer Owner { get; }

        public BetCard(Colour colour, IPlayer owner)
        {
            Colour = colour;
            Owner = owner;
        }
    }
}
