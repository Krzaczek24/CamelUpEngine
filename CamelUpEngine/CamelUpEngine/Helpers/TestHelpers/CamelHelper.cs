#if DEBUG
using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Helpers.TestHelpers
{
    public static class CamelHelper
    {
        public static IReadOnlyCollection<ICamel> AllCamels { get; } = ColourHelper.AllCamelColours.Select(colour => new Camel(colour)).Cast<ICamel>().ToList();
        public static IReadOnlyCollection<ICamel> GetCamels(params Colour[] colours) => AllCamels.Where(camel => colours.Contains(camel.Colour)).OrderBy(camel => colours.ToList().IndexOf(camel.Colour)).ToList();
        public static ICamel GetCamel(Colour colour) => AllCamels.Single(camel => camel.Colour == colour);
    }
}
#endif