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

        private Func<Guid> GenerateGuid { get; }
        internal Guid AudienceTileAvailableFieldsGuid { get; private set; }
        private Player currentPlayer;
        private readonly List<Player> players;
        private readonly List<Camel> camels;
        private readonly List<Field> fields;
        private readonly Dictionary<Colour, Field> camelPositions;
        private readonly Dicer dicer = new();
        private readonly TypingCardManager cardManager = new();

        public IPlayer CurrentPlayer => currentPlayer;
        public IReadOnlyCollection<IPlayer> Players => players.ToList();
        public IReadOnlyCollection<ICamel> Camels => camels.ToList();
        public IReadOnlyCollection<IField> Fields => fields.ToList();
        public IReadOnlyCollection<IAvailableField> AudienceTileAvailableFields => GetAudienceTileAvailableFields();
        public IReadOnlyCollection<IDrawnDice> DrawnDices => dicer.DrawnDices;
        public IReadOnlyCollection<IAvailableTypingCard> AvailableTypingCards => cardManager.AvailableCards;
        public IReadOnlyDictionary<Colour, int> CamelPositions => camelPositions.ToDictionary(entry => entry.Key, entry => entry.Value.Index);
        public IReadOnlyCollection<ICamel> CamelsOrder => fields.Reverse<IField>().SelectMany(field => field.Camels).ToList();

        public bool GameIsOver { get; private set; }
        public bool TurnIsOver => dicer.DrawnDices.Count() >= MaximalDrawnDices;

        public Game(IEnumerable<string> playerNames, bool randomizePlayersOrder = false, int fieldsCount = DefaultFieldsCount, Func<Guid> guidGenerationFunction = null)
        {
            GenerateGuid = guidGenerationFunction ?? Guid.NewGuid;
            AudienceTileAvailableFieldsGuid = GenerateGuid();

            players = GameInitializer.GeneratePlayers(playerNames, randomizePlayersOrder).ToList();
            camels = GameInitializer.GenerateCamels().ToList();
            fields = GameInitializer.GenerateFields(fieldsCount).ToList();
            camelPositions = GameInitializer.SetCamelsOnBoard(this);
            currentPlayer = players.First();
        }

        public ActionEvents DrawDice()
        {
            if (GameIsOver)
            {
                throw new GameOverException();
            }

            IDrawnDice dice = dicer.DrawDice();
            GiveCurrentPlayerACoin();
            MoveCamel(dice.Colour, dice.Value);

            if (GameIsOver)
            {
                FinishGame();
            }

            if (TurnIsOver)
            {
                GoToNextTurn();
            }

            SetNextPlayerAsCurrent();

            return ActionEventsCollector.GetEvents();
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

            return ActionEventsCollector.GetEvents();
        }

        public ActionEvents DrawTypingCard(IAvailableTypingCard card)
        {
            TypingCard typingCard = (TypingCard)cardManager.DrawCard(card);
            currentPlayer.AddTypingCard(typingCard);
            ActionEventsCollector.AddEvent(new TypingCardDrawnEvent(currentPlayer, typingCard));
            return ActionEventsCollector.GetEvents();
        }

        public ActionEvents PlaceAudienceTile(IAvailableField availableField, AudienceTileSide audienceTileSide)
        {
            if (GameIsOver)
            {
                throw new GameOverException();
            }

            if (AudienceTileAvailableFieldsGuid != availableField.DrawGuid)
            {
                throw new FieldExpiredAvailabilityException();
            }
            
            Field previousField = fields.SingleOrDefault(field => field.Index == availableField.Index - 1);
            Field nextField = fields.SingleOrDefault(field => field.Index == availableField.Index + 1);
            if (previousField?.AudienceTile != null || nextField?.AudienceTile != null)
            {
                throw new NearbyFieldOccupiedByAudienceTileException(availableField.Index);
            }

            Field previousAudienceTileField = fields.SingleOrDefault(field => field.AudienceTile?.Owner == currentPlayer);
            Field targetField = fields.Single(field => field.Index == availableField.Index);
            targetField.PutAudienceTile(currentPlayer.GetAudienceTile(audienceTileSide));
            previousAudienceTileField?.RemoveAudienceTile();

            SetNextPlayerAsCurrent();

            AudienceTileAvailableFieldsGuid = GenerateGuid();

            return ActionEventsCollector.GetEvents();
        }

        private void FinishGame()
        {
            SummarizeCurrentTurn();
            ActionEventsCollector.AddEvent(new GameOverEvent(this));
        }

        private void GoToNextTurn()
        {
            RemoveAllAudienceTiles();
            dicer.Reset();
            SummarizeCurrentTurn();
            ActionEventsCollector.AddEvent(new NewTurnEvent());
        }

        //TODO: otestować
        private void SummarizeCurrentTurn()
        {
            List<IPlayerTypingCardsReturnedEvent> playerTypingCardsReturnedEvents = new();
            var camelsOrder = CamelsOrder.GetColours().ToList();

            foreach (Player player in players)
            {
                int playerCoinsEarned = 0;
                foreach (ITypingCard typingCard in player.TypingCards)
                {
                    int rank = camelsOrder.IndexOf(typingCard.Colour);
                    switch (rank)
                    {
                        case 1: playerCoinsEarned += (int)typingCard.Value; break;
                        case 2: playerCoinsEarned++; break;
                        default: playerCoinsEarned--; break;
                    }
                }
                PlayerTypingCardsReturnedEvent actionEvent = new(player, player.ReturnTypingCards());
                playerTypingCardsReturnedEvents.Add(actionEvent);
                ActionEventsCollector.AddEvent(actionEvent);
                ActionEventsCollector.AddEvent(new CoinsAddedEvent(player, player.AddCoins(playerCoinsEarned)));
            }

            cardManager.Reset();
            ActionEventsCollector.AddEvent(new AllTypingCardsReturnedEvent(playerTypingCardsReturnedEvents));
        }

        private void GiveCurrentPlayerACoin() => ActionEventsCollector.AddEvent(new CoinsAddedEvent(currentPlayer, currentPlayer.AddCoins(Dicer.DiceDrawReward)));

        private void RemoveAllAudienceTiles() => fields.ForEach(field => field.RemoveAudienceTile());

        private void SetNextPlayerAsCurrent()
        {
            int currentIndex = players.ToList().IndexOf(currentPlayer);
            int playersCount = players.Count;
            currentPlayer = players.ElementAt(++currentIndex % playersCount);
            ActionEventsCollector.AddEvent(new ChangedCurrentPlayerEvent(currentPlayer));
        }

        private List<IAvailableField> GetAudienceTileAvailableFields()
        {
            List<IAvailableField> availableFields = new();
            
            for (int fieldIndex = 1; fieldIndex < fields.Count; fieldIndex++)
            {
                Field currentField = fields[fieldIndex];
                if (currentField.AudienceTile != null)
                {
                    fieldIndex++;
                    continue;
                }

                Field previousField = fields[fieldIndex - 1];
                Field nextField = fields[(fieldIndex + 1) % fields.Count];
                if (previousField.AudienceTile == null
                && !currentField.Camels.Any()
                && nextField.AudienceTile == null)
                {
                    availableFields.Add(new AvailableField(currentField.Index, AudienceTileAvailableFieldsGuid));
                }
            }

            return availableFields;
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
            if (DoesCamelGoThroughFinishLine(newFieldIndex))
            {
                PerformEndingCamelMove(newFieldIndex);
                GameIsOver = true;
                return;
            }
            ActionEventsCollector.AddEvent(new CamelsMovedEvent(camels, field.Index, newFieldIndex));
            field = fields[newFieldIndex - 1];

            // additional move if camel ended move on audience tile
            AudienceTile audienceTile = (AudienceTile)field.AudienceTile;
            if (audienceTile != null)
            {
                ActionEventsCollector.AddEvent(new CamelsStoodOnAudienceTileEvent(audienceTile));
                GiveCurrentPlayerACoin();
                newFieldIndex = field.Index + audienceTile.MoveValue;
                if (DoesCamelGoThroughFinishLine(newFieldIndex))
                {
                    PerformEndingCamelMove(newFieldIndex);
                    GameIsOver = true;
                    return;
                }
                ActionEventsCollector.AddEvent(new CamelsMovedEvent(camels, field.Index, newFieldIndex, audienceTile.Side.ToStackPutType()));
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
