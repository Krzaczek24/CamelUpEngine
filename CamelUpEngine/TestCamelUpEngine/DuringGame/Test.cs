using CamelUpEngine;
using NUnit.Framework;
using System.Collections.Generic;

namespace TestCamelUpEngine.DuringGame
{
    internal class Test
    {
        protected static IReadOnlyCollection<string> players = new[] { "Bezimienny", "Diego", "Gorn", "Milten", "Lester" };
        protected Game game = new(players);
    }
}
