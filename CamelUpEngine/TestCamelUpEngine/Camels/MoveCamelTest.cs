using CamelUpEngine;
using CamelUpEngine.Core.Actions.Steps;
using CamelUpEngine.Helpers;
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

            var drawDiceActionResult = game.DrawTheDice();
            var drawnDiceActionStep = drawDiceActionResult.GetActionStep<IDiceDrawnStep>();
            var drawnDice = drawnDiceActionStep.DrawnDice;

            int fieldIndexShift = drawnDice.Value;
            int camelInitialFieldIndex = camelsInitialFieldIndexes[drawnDice.Colour];
            int camelActualFieldIndex = game.CamelPositions[drawnDice.Colour];

            Assert.AreEqual(camelInitialFieldIndex + fieldIndexShift, camelActualFieldIndex);
        }
    }
}
