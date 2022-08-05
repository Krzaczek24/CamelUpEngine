#if DEBUG
using CamelUpEngine;
using CamelUpEngine.Core.Actions;
using CamelUpEngine.Core.Actions.Events;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Extensions;
using CamelUpEngine.GameObjects;
using CamelUpEngine.GameTools;
using CamelUpEngine.Helpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine.Camels
{
    internal class MoveCamelTest
    {
        private const int FIRST = 0;
        private const int MIDDLE = 3; // CamelMoveTester.Camels.Count / 2
        private const int LAST = 6; // CamelMoveTester.Camels.Count - 1
        private const int MEETUP_FIELD_INDEX = 10;
        private static IReadOnlyCollection<string> players = new[] { "Bezimienny", "Diego", "Gorn", "Milten", "Lester" };
        private CamelTrafficManager manager;

        [SetUp]
        public void SetUp()
        {
            manager = new(new Game(players).Fields);
        }

        [Test, Sequential, Repeat(10000)]
        public void TestCamelMovingInGame()
        {
            var dicer = new Dicer();

            for (int moves = 10; moves > 0; moves--)
            {
                TestCameSingleMove(dicer);
            }
        }

        private void TestCameSingleMove(Dicer dicer)
        {
            if (dicer.IsEmpty)
            {
                dicer.Reset();
            }

            var camelsInitialFieldIndexes = manager.CamelPositions;
            var drawnDice = dicer.DrawDice();

            var events = manager.MoveCamel(drawnDice.Colour, drawnDice.Value);
            var switchEvent = events.GetEvent<IMadCamelColourSwitchedEvent>();
            Colour currentColour = switchEvent == null ? drawnDice.Colour : ColourHelper.GetOppositeMadColour(drawnDice.Colour);

            int fieldIndexShift = drawnDice.Value;
            int camelInitialFieldIndex = camelsInitialFieldIndexes[currentColour];
            int camelActualFieldIndex = manager.CamelPositions[currentColour];

            Assert.AreEqual(camelInitialFieldIndex + fieldIndexShift, camelActualFieldIndex);
        }

        [Test, Sequential]
        public void TestMovingNth([Values(FIRST, MIDDLE, LAST)] int index)
        {
            MoveAllCamelsToTheSameField(MEETUP_FIELD_INDEX);
            const int shift = 1;

            ICamel camel = manager.AllCamelsOrder.ElementAt(index);
            var initialPositions = manager.CamelPositions;
            manager.MoveCamel(camel.Colour, shift);
            var newPositions = manager.CamelPositions;

            CollectionAssert.AreEqual(initialPositions.Select(p => p.Key), newPositions.Select(p => p.Key));
            Assert.That(initialPositions.Select(p => p.Value), Is.All.EqualTo(MEETUP_FIELD_INDEX));

            var movedCamelColours = manager.AllCamelsOrder.TakeUntil(c => c == camel, inclusive: true).Select(c => c.Colour).ToList();
            var notMovedCamelColours = manager.AllCamelsOrder.Select(c => c.Colour).Except(movedCamelColours).ToList();

            Assert.That(movedCamelColours, Has.Count.EqualTo(index + 1));
            Assert.That(notMovedCamelColours, Has.Count.EqualTo(manager.AllCamelsOrder.Count() - (index + 1)));

            var movedCamelPositions = newPositions.Where(p => movedCamelColours.Contains(p.Key)).Select(p => p.Value).ToList();
            var notMovedCamelPositions = newPositions.Where(p => notMovedCamelColours.Contains(p.Key)).Select(p => p.Value).ToList();

            Assert.That(movedCamelPositions, Is.All.EqualTo(MEETUP_FIELD_INDEX + shift));
            Assert.That(notMovedCamelPositions, Is.All.EqualTo(MEETUP_FIELD_INDEX));
        }

        private void MoveAllCamelsToTheSameField(int fieldIndex)
        {
            foreach (Colour colour in ColourHelper.AllCardColours)
            {
                int difference = fieldIndex - manager.CamelPositions[colour];
                manager.MoveCamel(colour, difference);
            }

            IActionEvent switchEvent = null;
            foreach (Colour colour in ColourHelper.MadColours)
            {
                Colour currentColour = switchEvent == null ? colour : ColourHelper.GetOppositeMadColour(colour);

                int difference = fieldIndex - manager.CamelPositions[currentColour];
                var events = manager.MoveCamel(currentColour, difference);
                switchEvent = events.GetEvent<IMadCamelColourSwitchedEvent>();
            }
        }
    }
}
#endif