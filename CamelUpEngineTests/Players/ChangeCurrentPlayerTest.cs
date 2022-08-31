using CamelUpEngine;
using CamelUpEngine.Core.Actions.Events;
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
            string expectedPlayerName; ;
            string newPlayerName = playersList.First();
            for (int i = 0; i < 10; i++)
            {
                expectedPlayerName = playersList[i % playersList.Count];
                Assert.AreEqual(expectedPlayerName, newPlayerName);
                Assert.AreEqual(expectedPlayerName, game.CurrentPlayer.Name);
                var changedPlayerEvent = game.DrawDice().SingleOrDefault(@event => @event is IChangedCurrentPlayerEvent) as IChangedCurrentPlayerEvent;
                if (changedPlayerEvent == null)
                {
                    Assert.IsTrue(game.TurnIsOver);
                    changedPlayerEvent = game.GoToNextTurn().Single(@event => @event is IChangedCurrentPlayerEvent) as IChangedCurrentPlayerEvent;
                }
                Assert.AreEqual(expectedPlayerName, changedPlayerEvent.PreviousPlayer.Name);
                newPlayerName = changedPlayerEvent.NewPlayer.Name;
            }
        }
    }
}
