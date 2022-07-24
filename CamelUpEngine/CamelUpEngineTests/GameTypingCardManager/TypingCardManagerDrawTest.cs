using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects;
using CamelUpEngine.GameTools;
using CamelUpEngine.Helpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine.GameTypingCardManager
{
    internal class TypingCardManagerDrawTest
    {
        private TypingCardManager manager = new();

        [SetUp]
        public void SetUp()
        {
            manager.Reset();
        }

        [Test]
        public void TestAvailableCards()
        {
            var expectedCards = TypingCardHelper.CardRepository.Where(card => card.Value == TypingCardValue.High).ToList();
            CollectionAssert.AreEquivalent(expectedCards, manager.AvailableCards);
        }

        [Test]
        public void TestDrawingCards()
        {
            var expectedCards = TypingCardHelper.CardRepository.Where(card => card.Value == TypingCardValue.High).ToList();
            Colour colour = Colour.Blue;

            CheckDrawSingleCard(colour, TypingCardValue.High, ref expectedCards);
            expectedCards.Add(TypingCardHelper.CardRepository.Single(card => card.Colour == colour && card.Value == TypingCardValue.Medium));
            CollectionAssert.AreEquivalent(expectedCards, manager.AvailableCards);

            CheckDrawSingleCard(colour, TypingCardValue.Medium, ref expectedCards);
            expectedCards.Add(TypingCardHelper.CardRepository.Single(card => card.Colour == colour && card.Value == TypingCardValue.Low));
            CollectionAssert.AreEquivalent(expectedCards, manager.AvailableCards);

            CheckDrawSingleCard(colour, TypingCardValue.Low, ref expectedCards);
            expectedCards.Add(TypingCardHelper.CardRepository.Single(card => card.Colour == colour && card.Value == TypingCardValue.Low));
            CollectionAssert.AreEquivalent(expectedCards, manager.AvailableCards);

            CheckDrawSingleCard(colour, TypingCardValue.Low, ref expectedCards);
            CollectionAssert.AreEquivalent(expectedCards, manager.AvailableCards);
        }

        private void CheckDrawSingleCard(Colour colour, TypingCardValue value, ref List<ITypingCard> expectedCards)
        {
            ITypingCard drawnCard = manager.DrawCard(colour);
            Assert.Multiple(() => {
                Assert.AreEqual(drawnCard.Colour, colour);
                Assert.AreEqual(drawnCard.Value, value);
            });
            expectedCards = expectedCards.Where(card => card.Colour != colour).ToList();
        }
    }
}