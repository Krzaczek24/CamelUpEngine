namespace CamelUpEngine
{
    public interface ICamel : IColourable
    {
        public bool IsMad { get; }
    }

    public sealed class Camel : ICamel
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
