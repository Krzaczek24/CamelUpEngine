using CamelUpEngine;
using CamelUpEngine.Core.Actions;
using CamelUpEngine.Core.Actions.Events;
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
        private Game game;

        [Test, Repeat(1000)]
        public void TestManyRandomGames()
        {
            game = new(new[] { "Bezimienny", "Diego", "Gorn", "Milten", "Lester" });

            Assert.DoesNotThrow(() =>
            {
                while (!game.GameIsOver)
                {
                    GetAvailableActions().GetRandom()();
                }
            });
            Assert.IsTrue(game.GameIsOver);
        }

        private IReadOnlyCollection<Func<ActionEvents>> GetAvailableActions()
        {
            List<Func<ActionEvents>> availableActions = new() { TestDrawingDice };

            if (game.AvailableTypingCards.Any())
            {
                availableActions.Add(TestDrawingTypingCard);
            }

            if (game.AudienceTileAvailableFields.Any())
            {
                availableActions.Add(TestPlacingAudienceTile);
            }

            if (false)
            {
                availableActions.Add(TestMakingBet);
            }

            return availableActions;
        }

        private ActionEvents TestDrawingDice()
        {
            ActionEvents events = game.DrawDice();

            CollectionAssert.AllItemsAreNotNull(events);
            CollectionAssert.AllItemsAreUnique(events);
            Assert.That(events, Has.One.AssignableTo<IDiceDrawnEvent>());

            if (events.Any(@event => @event is ICamelsStoodOnAudienceTileEvent))
            {
                Assert.That(events, Has.One.AssignableTo<ICamelsStoodOnAudienceTileEvent>());
                Assert.That(events, Has.Exactly(2).AssignableTo<ICoinsAddedEvent>());
                Assert.That(events, Has.Exactly(2).AssignableTo<ICamelMovedEvent>());
            }
            else
            {
                Assert.That(events, Has.One.AssignableTo<ICoinsAddedEvent>());
                Assert.That(events, Has.One.AssignableTo<ICamelMovedEvent>());
            }

            if (events.Any(@event => @event is IGameOverEvent))
            {
                Assert.That(events, Has.One.AssignableTo<IEndOfTurnEvent>());
                Assert.That(events, Has.None.AssignableTo<IAllAudienceTilesRemovementEvent>());
                Assert.That(events, Has.None.AssignableTo<IDicerRefilledEvent>());
                Assert.That(events, Has.One.AssignableTo<ICoinsCountingEvent>());
                Assert.That(events, Has.One.AssignableTo<IAllTypingCardsReturnedEvent>());
                Assert.That(events, Has.One.AssignableTo<IGameOverEvent>());
                Assert.That(events, Has.None.AssignableTo<INewTurnEvent>());
                Assert.That(events, Has.None.AssignableTo<IChangedCurrentPlayerEvent>());
            }
            else if (events.Any(@event => @event is IEndOfTurnEvent))
            {
                Assert.That(events, Has.One.AssignableTo<IEndOfTurnEvent>());
                Assert.That(events, Has.One.AssignableTo<IAllAudienceTilesRemovementEvent>());
                Assert.That(events, Has.One.AssignableTo<IDicerRefilledEvent>());
                Assert.That(events, Has.One.AssignableTo<ICoinsCountingEvent>());
                Assert.That(events, Has.One.AssignableTo<IAllTypingCardsReturnedEvent>());
                Assert.That(events, Has.One.AssignableTo<INewTurnEvent>());
                Assert.That(events, Has.One.AssignableTo<IChangedCurrentPlayerEvent>());
            }
            else
            {
                Assert.That(events, Has.One.AssignableTo<IChangedCurrentPlayerEvent>());
            }

            var eventsWithSubEvents = events.Where(@event => @event is IActionSubEvents).ToList();
            foreach (IActionSubEvents @event in eventsWithSubEvents)
            {
                Assert.That(@event.Count, Is.GreaterThanOrEqualTo(0).And.LessThanOrEqualTo(game.Players.Count), $"{nameof(IActionSubEvents)}.{nameof(@event.Count)}()");

                Type genericType = @event.GetType().GetInterfaces().Single(i => i.IsGenericType).GenericTypeArguments.First();
                switch (genericType.Name)
                {
                    case nameof(IAudienceTileRemovementEvent):
                        Assert.That((@event as IAllAudienceTilesRemovementEvent).SubEvents, Has.All.Matches<IAudienceTileRemovementEvent>(e => e.FieldIndex >= 1 && e.FieldIndex <= game.Fields.Count && e.AudienceTile != null));
                        break;
                    case nameof(ICoinsAddedEvent):
                        Assert.That((@event as ICoinsCountingEvent).SubEvents, Has.All.Matches<ICoinsAddedEvent>(e => e.CoinsCount != 0 && e.Player.Coins >= 0));
                        break;
                    case nameof(IPlayerTypingCardsReturnedEvent):
                        Assert.That((@event as IAllTypingCardsReturnedEvent).SubEvents, Has.All.Matches<IPlayerTypingCardsReturnedEvent>(e => e.TypingCards.Count > 0 && e.Player.TypingCards.Count == 0));
                        break;
                    default:
                        throw new NotImplementedException($"Not implemented type {genericType.Name}");
                }
            }

            return events;
        }

        private ActionEvents TestDrawingTypingCard()
        {
            // TODO: TestDrawingTypingCard
            ActionEvents events = game.DrawTypingCard(game.AvailableTypingCards.GetRandom());
            CollectionAssert.AllItemsAreNotNull(events);
            CollectionAssert.AllItemsAreUnique(events);
            Assert.That(events, Has.One.AssignableTo<IChangedCurrentPlayerEvent>());

            return events;
        }

        private ActionEvents TestPlacingAudienceTile()
        {
            // TODO: TestPlacingAudienceTile
            ActionEvents events = game.PlaceAudienceTile(game.AudienceTileAvailableFields.GetRandom(), Enum.GetValues<AudienceTileSide>().GetRandom());
            CollectionAssert.AllItemsAreNotNull(events);
            CollectionAssert.AllItemsAreUnique(events);
            Assert.That(events, Has.One.AssignableTo<IChangedCurrentPlayerEvent>());

            return events;
        }

        private ActionEvents TestMakingBet()
        {
            // TODO: TestMakingBet
            ActionEvents events = game.MakeBet(ColourHelper.AllCamelColours.GetRandom(), Enum.GetValues<BetType>().GetRandom());
            CollectionAssert.AllItemsAreNotNull(events);
            CollectionAssert.AllItemsAreUnique(events);
            Assert.That(events, Has.One.AssignableTo<IChangedCurrentPlayerEvent>());

            return events;
        }
    }
}
