using CamelUpEngine;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine.NewGame
{
    public class NewGameCamelsPositionsTest
    {
        private Game game;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var players = new[] { "Bezimienny", "Diego", "Gorn", "Milten", "Lester" };
            game = new Game(players);
        }

        [Test]
        public void TestIfGameGotAllField()
        {
            var actualFieldIndexes = game.Fields.Select(field => field.Index);
            var expectedIndexes = Enumerable.Range(1, Game.DEFAULT_FIELDS_COUNT);

            CollectionAssert.AreEqual(expectedIndexes, actualFieldIndexes);
        }

        [Test]
        public void TestIfAllCamelsAreOnboard()
        {
            var firstFields = game.Fields.Take(3);
            var lastFields = game.Fields.TakeLast(3);
            var fieldsToCheck = firstFields.Concat(lastFields);

            ICollection<ICamel> foundCamels = new List<ICamel>();
            foreach (IField field in fieldsToCheck)
            {
                field.GetCamels().ToList().ForEach(foundCamels.Add);
            }

            CollectionAssert.AllItemsAreUnique(game.Camels);
            CollectionAssert.AllItemsAreUnique(game.Camels.GetColours());
            CollectionAssert.AreEquivalent(game.Camels.GetColours(), ColourHelper.AllCamelColours);
            CollectionAssert.AreEquivalent(game.Camels, foundCamels);
        }
    }
}
