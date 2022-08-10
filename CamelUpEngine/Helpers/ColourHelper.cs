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
        public static IEnumerable<Colour> AllCardColours { get; } = AllColours.Except(MadColours).Except(new[] { Colour.Mad });
        public static IEnumerable<Colour> AllDiceColours { get; } = AllColours.Except(MadColours);
        public static IEnumerable<Colour> GetColours(this IEnumerable<IColourable> source) => source.Select(colourable => colourable.Colour).ToList();

        public static bool IsMadColour(Colour colour) => MadColours.Contains(colour);
        public static Colour GetOppositeMadColour(Colour colour) => MadColours.Except(new[] { colour }).Single();

        public static T GetSingle<T>(this IEnumerable<T> source, Colour colour, TypingCardValue value) where T : IColourable, IValuable => source.First(card => card.Colour == colour && card.Value == value);
        public static T GetSingle<T>(this IEnumerable<T> source, Colour colour) where T : IColourable => source.Single(item => item.Colour == colour);
        public static IReadOnlyCollection<T> GetMany<T>(this IEnumerable<T> source, params Colour[] colours) where T : IColourable => GetMany(source, colours.ToList());
        public static IReadOnlyCollection<T> GetMany<T>(this IEnumerable<T> source, IEnumerable<Colour> colours) where T : IColourable
        {
            return source.Where(item => colours.Contains(item.Colour)).OrderBy(item => colours.ToList().IndexOf(item.Colour)).ToList();
        }        
    }
}
