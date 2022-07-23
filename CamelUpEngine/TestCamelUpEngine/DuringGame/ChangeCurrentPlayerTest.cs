using NUnit.Framework;
using System.Linq;

namespace TestCamelUpEngine.DuringGame
{
    internal class ChangeCurrentPlayerTest : BaseClass
    {
        [Test]
        public void TestIfCurrentPlayerChanges()
        {
            var playersList = players.ToList();
            string expectedPlayerName;
            for (int i = 0; i < 10; i++)
            {
                expectedPlayerName = playersList[i % playersList.Count];
                Assert.AreEqual(expectedPlayerName, game.CurrentPlayer.Name);
                game.DrawTheDice();
            }
        }
    }
}
