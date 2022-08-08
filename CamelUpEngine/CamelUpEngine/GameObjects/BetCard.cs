using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.GameObjects
{
    public interface IBetCard : IColourable
    {
        public IPlayer Owner { get; }
    }

    internal class BetCard : IBetCard
    {
        public IPlayer Owner { get; }
        public Colour Colour { get; }

        public BetCard(IPlayer owner, Colour colour)
        {
            Owner = owner;
            Colour = colour;
        }

        public override string ToString() => $"{Owner.Name}'s {Colour.ToString().ToLower()} bet card";
    }
}
