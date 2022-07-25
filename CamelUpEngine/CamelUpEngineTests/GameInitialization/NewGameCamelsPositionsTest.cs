using CamelUpEngine;
using CamelUpEngine.GameObjects;
using CamelUpEngine.Helpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine.GameInitialization
{
    internal class NewGameCamelsPositionsTest
    {
        private static IReadOnlyCollection<string> players = new[] { "Bezimienny", "Diego", "Gorn", "Milten", "Lester" };
        private Game game;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            game = new Game(players);
        }

        [Test]
        public void TestIfGameGotAllField()
        {
            var actualFieldIndexes = game.Fields.Select(field => field.Index);
            var expectedIndexes = Enumerable.Range(1, Game.DefaultFieldsCount);

            CollectionAssert.AreEqual(expectedIndexes, actualFieldIndexes);
        }

        [Test]
        public void TestIfAllCamelsAreOnboard()
        {
            ICollection<ICamel> foundCamels = new List<ICamel>();

            var firstFields = game.Fields.Take(3);
            var lastFields = game.Fields.TakeLast(3);
            var fieldsToCheck = firstFields.Concat(lastFields).ToList();
            fieldsToCheck.ForEach(field => field.Camels.ToList().ForEach(foundCamels.Add));

            CollectionAssert.AllItemsAreUnique(game.Camels);
            CollectionAssert.AllItemsAreUnique(game.Camels.GetColours());
            CollectionAssert.AreEquivalent(game.Camels.GetColours(), ColourHelper.AllCamelColours);
            CollectionAssert.AreEquivalent(game.Camels, foundCamels);
        }
    }
}
