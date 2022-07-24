using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects;
using CamelUpEngine.GameTools;
using CamelUpEngine.Helpers;
using NUnit.Framework;

namespace TestCamelUpEngine.GameDicer
{
    internal class DicerDrawTest
    {
        [Test]
        public void TestDicerSingleDraw()
        {
            Dicer dicer = new Dicer();

            IDrawnDice dice;
            foreach (Colour _ in ColourHelper.AllDiceColours)
            {
                dice = dicer.DrawDice();
                CollectionAssert.Contains(dicer.DrawnDices, dice);
            }

            dice = dicer.DrawDice();
            Assert.IsNull(dice);
            Assert.Multiple(() =>
            {
                CollectionAssert.AllItemsAreNotNull(dicer.DrawnDices);
                CollectionAssert.AllItemsAreUnique(dicer.DrawnDices.GetColours());
                CollectionAssert.DoesNotContain(dicer.DrawnDices.GetColours(), Colour.Mad);
                CollectionAssert.IsNotSupersetOf(dicer.DrawnDices.GetColours(), ColourHelper.MadColours);
                CollectionAssert.IsSubsetOf(dicer.DrawnDices.GetColours(), ColourHelper.AllCamelColours);
            });

            dicer.Reset();
            CollectionAssert.IsEmpty(dicer.DrawnDices);
        }

        [Test]
        public void TestDicerInitialDraws()
        {
            var dices = Dicer.DrawDicesForInitialCamelsPlacement();
            Assert.Multiple(() =>
            {
                CollectionAssert.AllItemsAreNotNull(dices);
                CollectionAssert.AllItemsAreUnique(dices.GetColours());
                CollectionAssert.DoesNotContain(dices.GetColours(), Colour.Mad);
                CollectionAssert.IsSupersetOf(dices.GetColours(), ColourHelper.MadColours);
                CollectionAssert.AreEquivalent(dices.GetColours(), ColourHelper.AllCamelColours);
            });
        }
    }
}
