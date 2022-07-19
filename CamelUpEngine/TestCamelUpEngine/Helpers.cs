using CamelUpEngine.Core;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine
{
    internal static class Helpers
    {
        public static IEnumerable<Colour> GetColours(this IEnumerable<IColourable> colourables)
        {
            return colourables.Select(colourable => colourable.Colour).ToList();
        }
    }
}
