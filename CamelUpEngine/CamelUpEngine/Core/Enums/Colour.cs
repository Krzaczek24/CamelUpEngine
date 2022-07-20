using System;
using System.Collections.Generic;
using System.Linq;

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

    public static class ColourHelper
    {
        public static IEnumerable<Colour> MadColours { get; } = new[] { Colour.White, Colour.Black };
        public static IEnumerable<Colour> AllColours { get; } = Enum.GetValues<Colour>();
        public static IEnumerable<Colour> AllCamelColours { get; } = AllColours.Except(new[] { Colour.Mad });
        public static IEnumerable<Colour> AllDiceColours { get; } = AllColours.Except(MadColours);

        public static bool IsMadColour(Colour colour) => MadColours.Contains(colour);
    }
}
