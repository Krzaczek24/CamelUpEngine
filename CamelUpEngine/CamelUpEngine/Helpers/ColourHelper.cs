using CamelUpEngine.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Helpers
{
    public static class ColourHelper
    {
        public static IEnumerable<Colour> MadColours { get; } = new[] { Colour.White, Colour.Black };
        public static IEnumerable<Colour> AllColours { get; } = Enum.GetValues<Colour>();
        public static IEnumerable<Colour> AllCamelColours { get; } = AllColours.Except(new[] { Colour.Mad });
        public static IEnumerable<Colour> AllDiceColours { get; } = AllColours.Except(MadColours);
        public static IEnumerable<Colour> GetColours(this IEnumerable<IColourable> source) => source.Select(colourable => colourable.Colour).ToList();

        public static bool IsMadColour(Colour colour) => MadColours.Contains(colour);

        public static Colour GetOppositeMadColour(Colour colour) => MadColours.Except(new[] { colour }).Single();
    }
}
