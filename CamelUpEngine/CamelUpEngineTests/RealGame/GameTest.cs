using CamelUpEngine;
using CamelUpEngine.Core.Actions;
using CamelUpEngine.Core.Actions.Events;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Extensions;
using CamelUpEngine.Helpers;
using CamelUpEngine.Helpers.TestHelpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine.RealGame
{
    internal class GameTest
    {
        private IReadOnlyCollection<Func<ActionEvents>> actions;
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

        [Test, Repeat(1000)]
        public void TestManyRandomGames()
        {
            game = new(new[] { "Bezimienny", "Diego", "Gorn", "Milten", "Lester" });

            Assert.DoesNotThrow(() =>
            {
                while (!game.GameIsOver)
                {
                    actions.GetRandom()();
                }
            });
            Assert.IsTrue(game.GameIsOver);
        }

        private ActionEvents TestDrawingDice()
        {
            ActionEvents events = game.DrawDice();

            CollectionAssert.AllItemsAreNotNull(events);
            CollectionAssert.AllItemsAreUnique(events);
            Assert.That(events, Has.One.AssignableTo<IDiceDrawnEvent>());
            Assert.That(events.GetActionEvents<ICoinsAddedEvent>().Count(), Is.GreaterThanOrEqualTo(1).And.LessThanOrEqualTo(2), nameof(ICoinsAddedEvent));
            Assert.That(events.GetActionEvents<ICamelMovedEvent>().Count(), Is.GreaterThanOrEqualTo(1).And.LessThanOrEqualTo(2), nameof(ICamelMovedEvent));
            //TODO: obczaić resztę przypadków - jeśli koniec gry - jesli koniec tury - itd.
            Assert.That(events, Has.One.AssignableTo<IChangedCurrentPlayerEvent>());

            return events;
        }

        private ActionEvents TestDrawingTypingCard()
        {
            var card = game.AvailableTypingCards.GetRandom();
            if (card == null)
            {
                return null;
            }

            // TODO: TestDrawingTypingCard
            ActionEvents events = game.DrawTypingCard(card);
            CollectionAssert.AllItemsAreNotNull(events);
            CollectionAssert.AllItemsAreUnique(events);
            Assert.That(events, Has.One.AssignableTo<IChangedCurrentPlayerEvent>());

            return events;
        }

        private ActionEvents TestPlacingAudienceTile()
        {
            var field = game.AudienceTileAvailableFields.GetRandom();
            if (field == null)
            {
                return null;
            }

            // TODO: TestPlacingAudienceTile
            ActionEvents events = game.PlaceAudienceTile(field, Enum.GetValues<AudienceTileSide>().GetRandom());
            CollectionAssert.AllItemsAreNotNull(events);
            CollectionAssert.AllItemsAreUnique(events);
            Assert.That(events, Has.One.AssignableTo<IChangedCurrentPlayerEvent>());

            return events;
        }

        private ActionEvents TestMakingBet()
        {
            ActionEvents events = game.MakeBet(ColourHelper.AllCamelColours.GetRandom(), Enum.GetValues<BetType>().GetRandom());

            // TODO: TestMakingBet
            CollectionAssert.AllItemsAreNotNull(events);
            CollectionAssert.AllItemsAreUnique(events);
            Assert.That(events, Has.One.AssignableTo<IChangedCurrentPlayerEvent>());

            return events;
        }
    }
}
