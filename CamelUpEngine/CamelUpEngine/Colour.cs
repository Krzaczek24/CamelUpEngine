using System;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine
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

    public static class ColourHelper
    {
        public static IEnumerable<Colour> MadColours { get; } = new[] { Colour.White, Colour.Black };
        public static IEnumerable<Colour> AllCamelColours { get; } = Enum.GetValues<Colour>().Except(new[] { Colour.Mad });
        public static IEnumerable<Colour> AllDiceColours { get; } = Enum.GetValues<Colour>().Except(MadColours);

        public static bool IsMadColour(Colour colour) => MadColours.Contains(colour);
    }
}
