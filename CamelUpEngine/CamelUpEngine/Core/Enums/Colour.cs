namespace CamelUpEngine.Core.Enums
{
    public enum Colour
    {
        Red,
        Yellow,
        Green,
        Blue,
        Violet,
        White,
        Black,
        Mad
    }

    public interface IColourable
    {
        public Colour Colour { get; }
    }
}
