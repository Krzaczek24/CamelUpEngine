using CamelUpEngine;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Extensions;
using CamelUpEngine.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine.RealGame
{
    internal class GameTest
    {
        private static IReadOnlyCollection<string> players = new[] { "Bezimienny", "Diego", "Gorn", "Milten", "Lester" };
        private IReadOnlyCollection<Action> actions;
        private Game game;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            actions = new[] {
                TestDrawingDice,
                TestDrawingTypingCard,
                TestPlacingAudienceTile,
                TestMakingBet
            }.ToList();
        }

        [SetUp]
        public void SetUp()
        {
            game = new(players);
        }

        [Test, Repeat(100)]
        public void TestGame()
        {
            Assert.DoesNotThrow(() =>
            {
                while (!game.GameIsOver)
                {
                    actions.GetRandom()();
                }
            });
            Assert.IsTrue(game.GameIsOver);
        }

        private void TestDrawingDice()
        {
            game.DrawDice();

            // TODO: TestDrawingDice
        }

        private void TestDrawingTypingCard()
        {
            game.DrawTypingCard(game.AvailableTypingCards.GetRandom());

            // TODO: TestDrawingTypingCard
        }

        private void TestPlacingAudienceTile()
        {
            var field = game.AudienceTileAvailableFields.GetRandom();
            if (field != null)
            {
                game.PlaceAudienceTile(game.AudienceTileAvailableFields.GetRandom(), Enum.GetValues<AudienceTileSide>().GetRandom());
            }

            // TODO: TestPlacingAudienceTile
        }

        private void TestMakingBet()
        {
            game.MakeBet(ColourHelper.AllCamelColours.GetRandom(), Enum.GetValues<BetType>().GetRandom());

            // TODO: TestMakingBet
        }
    }
}
