using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects;
using CamelUpEngine.GameTools;
using CamelUpEngine.Helpers.TestHelpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngineTests.GameTypingCardsManager
{
    public class TypingCardsManagerCoinsCountTest
    {
        private List<ICamel> camels = CamelHelper.GetCamels(Colour.Green, Colour.White, Colour.Red, Colour.Black, Colour.Blue, Colour.Yellow, Colour.Violet).ToList();
        private List<ITypingCard> typingCards = new();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            TypingCardsManager manager = new(() => Guid.Empty);

            while (manager.AvailableCards.Any())
            {
                typingCards.Add(manager.DrawCard(manager.AvailableCards.First()));
            }
        }

        [Test]
        public void TestGettingCoinsWhenNoTypingCards()
        {
            List<ITypingCard> cards = new List<ITypingCard>();

            const int result = 0;
            Assert.AreEqual(result, TypingCardsManager.CountCoins(camels, cards));
        }

        [Test]
        public void TestGettingCoinsForFirstAndSecondCamelWhenBetweenThemIsMadCamel()
        {
            List<ITypingCard> cards = new List<ITypingCard>()
            {
                typingCards.GetCard(Colour.Green, TypingCardValue.High),
                typingCards.GetCard(Colour.Green, TypingCardValue.Medium),
                typingCards.GetCard(Colour.Red, TypingCardValue.High)
            };

            const int result = 5 + 3 + 1;
            Assert.AreEqual(result, TypingCardsManager.CountCoins(camels, cards));
        }

        [Test]
        public void TestGettingCoinsForLastCamel()
        {
            List<ITypingCard> cards = new List<ITypingCard>()
            {
                typingCards.GetCard(Colour.Violet, TypingCardValue.High)
            };

            const int result = -1;
            Assert.AreEqual(result, TypingCardsManager.CountCoins(camels, cards));
        }

        [Test]
        public void TestGettingCoinsForAllCamels()
        {
            List<ITypingCard> cards = camels.Where(camel => !camel.IsMad).Select(camel => typingCards.GetCard(camel.Colour, TypingCardValue.High)).ToList();

            const int result = 5 + 1 - 1 - 1 - 1;
            Assert.AreEqual(result, TypingCardsManager.CountCoins(camels, cards));
        }

        [Test]
        public void TestGettingCoinsForSecondCamel()
        {
            List<ITypingCard> cards = new List<ITypingCard>()
            {
                typingCards.GetCard(Colour.Red, TypingCardValue.Medium),
                typingCards.GetCard(Colour.Red, TypingCardValue.Low)
            };

            const int result = 1 + 1;
            Assert.AreEqual(result, TypingCardsManager.CountCoins(camels, cards));
        }

        [Test]
        public void TestGettingCoinsForTwoTheSameCards()
        {
            List<ITypingCard> cards = new List<ITypingCard>()
            {
                typingCards.GetCard(Colour.Blue, TypingCardValue.Low),
                typingCards.GetCard(Colour.Blue, TypingCardValue.Low)
            };

            const int result = - 1 - 1;
            Assert.AreEqual(result, TypingCardsManager.CountCoins(camels, cards));
        }

        [Test]
        public void TestGettingCoinsForFirstCamelAfterTwoMad()
        {
            var camels = CamelHelper.GetCamels(Colour.White, Colour.Black, Colour.Yellow).ToList();

            List<ITypingCard> cards = new List<ITypingCard>()
            {
                typingCards.GetCard(Colour.Yellow, TypingCardValue.Medium)
            };

            const int result = 3;
            Assert.AreEqual(result, TypingCardsManager.CountCoins(camels, cards));
        }
    }
}
