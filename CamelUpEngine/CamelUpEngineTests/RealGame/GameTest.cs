using CamelUpEngine;
using CamelUpEngine.Core.Actions;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Extensions;
using CamelUpEngine.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestCamelUpEngine.RealGame
{
    internal class GameTest
    {
        private const int ACTIONS_COUNT = 4;
        private static IReadOnlyCollection<string> players = new[] { "Bezimienny", "Diego", "Gorn", "Milten", "Lester" };
        private Game game;
        private Random random;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            random = new Random();
        }

        [SetUp]
        public void SetUp()
        {
            game = new(players);
        }

        [Test, Repeat(100)]
        public void TestGame()
        {
            Assert.Multiple(() =>
            {
                Assert.DoesNotThrow(() =>
                {
                    while (!game.GameIsOver)
                    {
                        game.DrawDice();
                    }
                });
                Assert.IsTrue(game.GameIsOver);
            });
        }

        private IActionResult DoRandomAction()
        {
            switch(random.Next(ACTIONS_COUNT))
            {
                case 0: return game.DrawDice();
                case 1: return game.DrawTypingCard(ColourHelper.AllCamelColours.GetRandom());
                case 2: return game.PlaceAudienceTile(game.Fields.Select(field => field.Index).GetRandom(), Enum.GetValues<AudienceTileSide>().GetRandom());
                case 3: return game.MakeBet(ColourHelper.AllCamelColours.GetRandom(), Enum.GetValues<BetType>().GetRandom());
                default: throw new InvalidOperationException();
            }
        }
    }
}
