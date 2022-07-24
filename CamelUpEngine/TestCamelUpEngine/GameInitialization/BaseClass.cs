using CamelUpEngine;
using NUnit.Framework;
using System.Collections.Generic;

namespace TestCamelUpEngine.GameInitialization
{
    internal abstract class BaseClass
    {
        protected static IReadOnlyCollection<string> players = new[] { "Bezimienny", "Diego", "Gorn", "Milten", "Lester" };
        protected Game game;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            game = new Game(players);
        }
    }
}
