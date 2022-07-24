using CamelUpEngine;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine.Players
{
    internal class ChangeCurrentPlayerTest
    {
        protected static IReadOnlyCollection<string> players = new[] { "Bezimienny", "Diego", "Gorn", "Milten", "Lester" };
        protected Game game = new(players);

        [Test]
        public void TestIfCurrentPlayerChanges()
        {
            var playersList = players.ToList();
            string expectedPlayerName;
            for (int i = 0; i < 10; i++)
            {
                expectedPlayerName = playersList[i % playersList.Count];
                Assert.AreEqual(expectedPlayerName, game.CurrentPlayer.Name);
                game.DrawDice();
            }
        }
    }
}
