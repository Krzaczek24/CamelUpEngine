using CamelUpEngine.Core.Actions;
using CamelUpEngine.Core.Actions.Events;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions;
using CamelUpEngine.GameObjects;
using CamelUpEngine.GameTools;
using CamelUpEngine.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine
{
    public sealed class Game
    {
        public const int MinimalPlayersCount = 3;
        public const int MaximalPlayersCount = 8;
        public const int DefaultFieldsCount = 16;
        public const int MaximalDrawnDices = 5;

        private Player currentPlayer;
        private readonly List<Player> players;
        private readonly List<Camel> camels;
        private readonly List<Field> fields;
        private readonly Dictionary<Colour, Field> camelPositions;
        private readonly Dicer dicer;
        private readonly TypingCardsManager cardManager;
        private readonly AudienceTilesManager tilesManager;

        public ActionEvents History => ActionEventsCollector.GetGameEvents();
        public IPlayer CurrentPlayer => currentPlayer;
        public IReadOnlyCollection<IPlayer> Players => players.ToList();
        public IReadOnlyCollection<ICamel> Camels => camels.ToList();
        public IReadOnlyCollection<IField> Fields => fields.ToList();
        public IReadOnlyCollection<IDrawnDice> DrawnDices => dicer.DrawnDices;
        public IReadOnlyCollection<IAvailableField> AudienceTileAvailableFields => tilesManager.GetAudienceTileAvailableFields();
        public IReadOnlyCollection<IAvailableTypingCard> AvailableTypingCards => cardManager.AvailableCards;
        public IReadOnlyDictionary<Colour, int> CamelPositions => camelPositions.ToDictionary(entry => entry.Key, entry => entry.Value.Index);
        public IReadOnlyCollection<ICamel> CamelsOrder => fields.Reverse<IField>().SelectMany(field => field.Camels).ToList();

        public bool GameIsOver { get; private set; }
        public bool TurnIsOver => dicer.DrawnDices.Count() >= MaximalDrawnDices;

        public Game(IEnumerable<string> playerNames, bool randomizePlayersOrder = false, int fieldsCount = DefaultFieldsCount)
        {
            ActionEventsCollector.Reset();
            players = GameInitializer.GeneratePlayers(playerNames, randomizePlayersOrder).ToList();
            camels = GameInitializer.GenerateCamels().ToList();
            fields = GameInitializer.GenerateFields(fieldsCount).ToList();
            camelPositions = GameInitializer.SetCamelsOnBoard(this);
            dicer = new();
            cardManager = new();
            tilesManager = new(fields);
            currentPlayer = players.First();
        }

        public ActionEvents DrawDice()
        {
            if (GameIsOver)
            {
                throw new GameOverException();
            }

            IDrawnDice drawnDice = dicer.DrawDice();
            ActionEventsCollector.AddEvent(new DiceDrawnEvent(drawnDice));
            ActionEventsCollector.AddEvent(new CoinsAddedEvent(currentPlayer, currentPlayer.AddCoins(1)));

            MoveCamel(drawnDice.Colour, drawnDice.Value);

            if (GameIsOver)
            {
                FinishGame();
            }

            if (TurnIsOver)
            {
                GoToNextTurn();
            }

            SetNextPlayerAsCurrent();

            return ActionEventsCollector.GetActionEvents();
        }

        public ActionEvents MakeBet(Colour colour, BetType betType)
        {
            if (GameIsOver)
            {
                throw new GameOverException();
            }

            // TODO: zabranie karty graczowi
            // TODO: położenie karty na odpowiednim stosie

            SetNextPlayerAsCurrent();

            return ActionEventsCollector.GetActionEvents();
        }

        public ActionEvents DrawTypingCard(IAvailableTypingCard card)
        {
            TypingCard typingCard = (TypingCard)cardManager.DrawCard(card);
            currentPlayer.AddTypingCard(typingCard);
            ActionEventsCollector.AddEvent(new TypingCardDrawnEvent(currentPlayer, typingCard));

            SetNextPlayerAsCurrent();

            return ActionEventsCollector.GetActionEvents();
        }

        public ActionEvents PlaceAudienceTile(IAvailableField availableField, AudienceTileSide audienceTileSide)
        {
            if (GameIsOver)
            {
                throw new GameOverException();
            }

            var placementAudienceTileEvent = tilesManager.PlaceAudienceTile(currentPlayer, availableField, audienceTileSide, out var removementAudienceTileEvent);
            if (removementAudienceTileEvent != null)
            {
                ActionEventsCollector.AddEvent(removementAudienceTileEvent);
            }
            ActionEventsCollector.AddEvent(placementAudienceTileEvent);

            SetNextPlayerAsCurrent();

            return ActionEventsCollector.GetActionEvents();
        }

        private void FinishGame()
        {
            ActionEventsCollector.AddEvent(new GameOverEvent(this));
            SummarizeCurrentTurn();
        }

        private void GoToNextTurn()
        {
            ActionEventsCollector.AddEvent(new EndOfTurnEvent());
            RemoveAllAudienceTiles();
            dicer.Reset();
            SummarizeCurrentTurn();
            ActionEventsCollector.AddEvent(new NewTurnEvent());
        }

        private void SummarizeCurrentTurn()
        {
            List<ICoinsAddedEvent> playerCoinsEarnedEvents = new();
            List<IPlayerTypingCardsReturnedEvent> playerTypingCardsReturnedEvents = new();

            foreach (Player player in players)
            {
                IReadOnlyCollection<ITypingCard> returnedTypingCard = player.ReturnTypingCards();
                if (returnedTypingCard.Any())
                {
                    playerTypingCardsReturnedEvents.Add(new PlayerTypingCardsReturnedEvent(player, returnedTypingCard));
                    int earnedCoins = TypingCardsManager.CountCoins(CamelsOrder, player.TypingCards);
                    if (earnedCoins != 0)
                    {
                        playerCoinsEarnedEvents.Add(new CoinsAddedEvent(player, player.AddCoins(earnedCoins)));
                    }
                }
            }
            ActionEventsCollector.AddEvent(new CoinsCountingEvent(playerCoinsEarnedEvents));

            cardManager.Reset();
            ActionEventsCollector.AddEvent(new AllTypingCardsReturnedEvent(playerTypingCardsReturnedEvents));
        }

        private void RemoveAllAudienceTiles()
        {
            var fieldsWithAudienceTile = fields.Where(field => field.AudienceTile != null);
            foreach (Field field in fieldsWithAudienceTile)
            {
                field.RemoveAudienceTile();
                ActionEventsCollector.AddEvent(new AudienceTileRemovementEvent(field.Index, field.AudienceTile));
            }
        }

        private void SetNextPlayerAsCurrent()
        {
            int currentIndex = players.ToList().IndexOf(currentPlayer);
            int playersCount = players.Count;
            currentPlayer = players.ElementAt(++currentIndex % playersCount);
            ActionEventsCollector.AddEvent(new ChangedCurrentPlayerEvent(currentPlayer));
        }

        private void MoveCamel(Colour colour, int value)
        {
            bool isMadColour = ColourHelper.IsMadColour(colour);
            if (isMadColour && ShouldSwitchMadColour(colour))
            {
                colour = ColourHelper.GetOppositeMadColour(colour);
            }

            // camel move
            Field field = camelPositions[colour];
            List<Camel> camels = field.TakeOffCamel(colour);
            int newFieldIndex = field.Index + value;
            ActionEventsCollector.AddEvent(new CamelsMovedEvent(camels, field.Index, newFieldIndex));
            if (DoesCamelGoThroughFinishLine(newFieldIndex))
            {
                PerformEndingCamelMove(newFieldIndex);
                GameIsOver = true;
                return;
            }
            field = fields[newFieldIndex - 1];

            // additional move if camel ended move on audience tile
            AudienceTile audienceTile = (AudienceTile)field.AudienceTile;
            if (audienceTile != null)
            {
                ActionEventsCollector.AddEvent(new CamelsStoodOnAudienceTileEvent(audienceTile));
                ActionEventsCollector.AddEvent(new CoinsAddedEvent(audienceTile.Owner, ((Player)audienceTile.Owner).AddCoins(1)));
                newFieldIndex = field.Index + audienceTile.MoveValue;
                ActionEventsCollector.AddEvent(new CamelsMovedEvent(camels, field.Index, newFieldIndex, audienceTile.Side.ToStackPutType()));
                if (DoesCamelGoThroughFinishLine(newFieldIndex))
                {
                    PerformEndingCamelMove(newFieldIndex);
                    GameIsOver = true;
                    return;
                }
                field = fields[newFieldIndex - 1];
                if (audienceTile.Side == AudienceTileSide.Booing)
                {
                    field.PutCamels(camels, StackPutType.Bottom);
                    camels.ForEach(camel => camelPositions[camel.Colour] = field);
                    return;
                }
            }

            field.PutCamels(camels);
            camels.ForEach(camel => camelPositions[camel.Colour] = field);
        }

        private void PerformEndingCamelMove(int newFieldIndex)
        {
            Field field;
            if (newFieldIndex <= 0)
            {
                (field = fields.First()).PutCamels(camels, StackPutType.Bottom);                
            }
            else
            {
                (field = fields.Last()).PutCamels(camels);
            }
            camels.ForEach(camel => camelPositions[camel.Colour] = field);
        }

        private bool DoesCamelGoThroughFinishLine(int newFieldIndex) => newFieldIndex <= 0 || newFieldIndex > fields.Count;

        private bool ShouldSwitchMadColour(Colour colour)
        {
            if (AreMadCamelsOnTheSameField() && IsNearestCamelOnBackMad(colour))
            {
                ActionEventsCollector.AddEvent(new MadCamelColourSwitchedEvent(MadCamelColourSwitchReason.OtherMadCamelIsDirectlyOnBackOfOtherOne));
                return true;
            }

            if (!AnyNotMadCamelsOnBack(colour) && AnyNotMadCamelsOnBack(ColourHelper.GetOppositeMadColour(colour)))
            {
                ActionEventsCollector.AddEvent(new MadCamelColourSwitchedEvent(MadCamelColourSwitchReason.OnlyOneMadCamelIsCarryingNonMadCamels));
                return true;
            }

            return false;
        }

        private bool IsNearestCamelOnBackMad(Colour colour) => camelPositions[colour].Camels.TakeWhile(camel => camel.Colour == colour).LastOrDefault()?.IsMad ?? false;
        private bool AnyNotMadCamelsOnBack(Colour colour) => camelPositions[colour].Camels.TakeWhile(camel => camel.Colour == colour).Where(camel => !camel.IsMad).Any();
        private bool AreMadCamelsOnTheSameField() => camelPositions[Colour.White] == camelPositions[Colour.Black];
    }
}
