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

            if (events.Any(@event => @event is IMadCamelColourSwitchedEvent))
            {
                Assert.That(events, Has.One.AssignableTo<IMadCamelColourSwitchedEvent>());
                var switchColourEvent = events.GetEvent<IMadCamelColourSwitchedEvent>();
                Assert.That(switchColourEvent.SwitchReason, Is.Not.EqualTo(MadCamelColourSwitchReason.UNDEFINED));
                Assert.That(switchColourEvent.From, Is.Not.EqualTo(switchColourEvent.To));
                CollectionAssert.Contains(ColourHelper.MadColours, switchColourEvent.From);
                CollectionAssert.Contains(ColourHelper.MadColours, switchColourEvent.To);
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

                Assert.That(game.Players.Select(player => player.TypingCards), Has.All.Empty);
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
                
                Assert.That(game.Fields.Select(field => field.AudienceTile), Has.All.Null);
                Assert.That(game.DrawnDices, Is.Empty);
                Assert.That(game.Players.Select(player => player.TypingCards), Has.All.Empty);
            }
            else
            {
                Assert.That(events, Has.One.AssignableTo<IChangedCurrentPlayerEvent>());
            }

            var eventsWithSubEvents = events.GetEvents<IActionSubEvents>().ToList();
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

            var camelsInGame = game.Fields.SelectMany(field => field.Camels).ToList();
            CollectionAssert.AllItemsAreNotNull(camelsInGame);
            CollectionAssert.AllItemsAreUnique(camelsInGame);
            CollectionAssert.AreEquivalent(game.Camels, camelsInGame);
            CollectionAssert.IsSubsetOf(camelsInGame, game.Camels);

            return events;
        }

        private ActionEvents TestDrawingTypingCard()
        {
            var availableCards = game.AvailableTypingCards;
            ActionEvents events = game.DrawTypingCard(availableCards.GetRandom());

            CollectionAssert.AllItemsAreNotNull(events);
            CollectionAssert.AllItemsAreUnique(events);

            Assert.That(events, Has.One.AssignableTo<ITypingCardDrawnEvent>());
            Assert.That(events, Has.One.AssignableTo<IChangedCurrentPlayerEvent>());

            var drawCardEvent = events.GetEvent<ITypingCardDrawnEvent>();
            Assert.AreEqual(events.GetEvent<IChangedCurrentPlayerEvent>().PreviousPlayer, drawCardEvent.Player);
            CollectionAssert.Contains(availableCards, drawCardEvent.TypingCard);
            if (drawCardEvent.TypingCard.Value != TypingCardValue.Low)
            {
                CollectionAssert.DoesNotContain(game.AvailableTypingCards, drawCardEvent.TypingCard);
            }
            else
            {
                Assert.That(game.AvailableTypingCards.Where(card => card == drawCardEvent.TypingCard).Count(), Is.GreaterThanOrEqualTo(0).And.LessThanOrEqualTo(1));
            }

            return events;
        }

        private ActionEvents TestPlacingAudienceTile()
        {
            var player = game.CurrentPlayer;
            bool playerAlreadPlacedAudienceTile = game.Fields.Any(field => field.AudienceTile?.Owner == game.CurrentPlayer);
            var field = game.AudienceTileAvailableFields.GetRandom();
            var tileSide = Enum.GetValues<AudienceTileSide>().GetRandom();
            ActionEvents events = game.PlaceAudienceTile(field, tileSide);

            CollectionAssert.AllItemsAreNotNull(events);
            CollectionAssert.AllItemsAreUnique(events);

            Assert.That(events, Has.One.AssignableTo<IAudienceTilePlacementEvent>());
            if (playerAlreadPlacedAudienceTile)
            {
                Assert.That(events, Has.One.AssignableTo<IAudienceTileRemovementEvent>());
                var @event = events.GetEvent<IAudienceTileRemovementEvent>();
                Assert.AreEqual(player, @event.AudienceTile.Owner);
            }
            else
            {
                Assert.That(events, Has.None.AssignableTo<IAudienceTileRemovementEvent>());
            }
            Assert.That(events, Has.One.AssignableTo<IChangedCurrentPlayerEvent>());

            var placementEvent = events.GetEvent<IAudienceTilePlacementEvent>();
            Assert.AreEqual(field.Index, placementEvent.FieldIndex);
            Assert.AreEqual(tileSide, placementEvent.AudienceTile.Side);
            Assert.AreEqual(player, placementEvent.AudienceTile.Owner);

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