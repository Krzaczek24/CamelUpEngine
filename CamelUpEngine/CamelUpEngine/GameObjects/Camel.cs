using CamelUpEngine.Core;

namespace CamelUpEngine.GameObjects
{
    public interface ICamel : IColourable
    {
        public bool IsMad { get; }
    }

    internal sealed class Camel : ICamel
    {
        public Colour Colour { get; }
        public bool IsMad => ColourHelper.IsMadColour(Colour);

        private Camel() { }
        public Camel(Colour colour)
        {
            Colour = colour;
        }

        public override string ToString()
        {
            return $"{Colour} camel";
        }
    }
}
