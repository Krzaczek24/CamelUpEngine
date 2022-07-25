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
            var expectedTopCards = TypingCardHelper.GetCards(TypingCardValue.High).ToList();

            foreach (Colour colour in ColourHelper.AllCardColours)
            {
                var referenceStack = TypingCardHelper.GetStack(colour);
                while (referenceStack.Any())
                {
                    CheckDrawSingleCard(ref referenceStack, ref expectedTopCards);
                }
            }
        }

        private void CheckDrawSingleCard(ref Stack<ITypingCard> expectedStack, ref List<ITypingCard> expectedTopCards)
        {
            ITypingCard expectedCard = expectedStack.Pop();
            ITypingCard drawnCard = manager.DrawCard(expectedCard.Colour);

            expectedTopCards = expectedTopCards.Where(card => card != expectedCard).ToList();
            if (expectedStack.TryPeek(out ITypingCard nextCard))
            {
                expectedTopCards.Add(nextCard);
            }

            Assert.AreEqual(drawnCard, expectedCard);
            CollectionAssert.AreEquivalent(expectedTopCards, manager.AvailableCards);
        }
    }
}