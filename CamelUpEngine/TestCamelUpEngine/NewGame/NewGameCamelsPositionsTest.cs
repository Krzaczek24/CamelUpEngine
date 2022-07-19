using CamelUpEngine;
using CamelUpEngine.Core;
using CamelUpEngine.GameObjects;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine.NewGame
{
    internal class NewGameCamelsPositionsTest : BaseClass
    {
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
