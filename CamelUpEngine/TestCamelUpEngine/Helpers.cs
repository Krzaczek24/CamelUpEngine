using CamelUpEngine;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine
{
    internal static class Helpers
    {
        public static IEnumerable<Colour> GetColours(this IEnumerable<ICamel> camels)
        {
            return camels.Select(camel => camel.Colour).ToList();
        }
    }
}
