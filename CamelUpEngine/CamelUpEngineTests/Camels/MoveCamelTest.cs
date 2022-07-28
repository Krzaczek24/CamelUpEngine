#if DEBUG
using CamelUpEngine;
using CamelUpEngine.Core.Actions.Events;
using CamelUpEngine.Helpers.TestHelpers;
using NUnit.Framework;
using System.Collections.Generic;

namespace TestCamelUpEngine.Camels
{
    internal class MoveCamelTest
    {
        protected static IReadOnlyCollection<string> players = new[] { "Bezimienny", "Diego", "Gorn", "Milten", "Lester" };
        protected Game game = new(players);

        [Test, Sequential]
        public void TestCamelMovingInGame()
        {
            for (int moves = 3; moves > 0; moves--)
            {
                TestCameSingleMove();
            }
        }

        private void TestCameSingleMove()
        {
            var camelsInitialFieldIndexes = game.CamelPositions;

            var drawDiceActionResult = game.DrawDice();
            var drawnDiceActionEvent = drawDiceActionResult.GetActionEvent<IDiceDrawnEvent>();
            var drawnDice = drawnDiceActionEvent.DrawnDice;

            int fieldIndexShift = drawnDice.Value;
            int camelInitialFieldIndex = camelsInitialFieldIndexes[drawnDice.Colour];
            int camelActualFieldIndex = game.CamelPositions[drawnDice.Colour];

            Assert.AreEqual(camelInitialFieldIndex + fieldIndexShift, camelActualFieldIndex);
        }
    }
}
#endif