using CamelUpEngine.Core.Actions;
using CamelUpEngine.Core.Actions.Steps;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions;
using CamelUpEngine.GameObjects;
using CamelUpEngine.GameTools;
using CamelUpEngine.Helpers;
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
        private readonly Dicer dicer = new();
        private readonly TypingCardManager cardManager = new();

        public IPlayer CurrentPlayer => currentPlayer;
        public IReadOnlyCollection<IPlayer> Players => players.ToList();
        public IReadOnlyCollection<ICamel> Camels => camels.ToList();
        public IReadOnlyCollection<IField> Fields => fields.ToList();
        public IReadOnlyCollection<IAvailableField> AudienceTileAvailableFields => throw new System.NotImplementedException(); // TODO: zaimplementować AudienceTileAvailableFields
        public IReadOnlyCollection<IDrawnDice> DrawnDices => dicer.DrawnDices;
        public IReadOnlyCollection<IAvailableTypingCard> AvailableTypingCards => cardManager.AvailableCards;
        public IReadOnlyDictionary<Colour, int> CamelPositions => camelPositions.ToDictionary(entry => entry.Key, entry => entry.Value.Index);

        public bool GameIsOver { get; private set; }
        public bool TurnIsOver => dicer.DrawnDices.Count() >= MaximalDrawnDices;

        public Game(IEnumerable<string> playerNames, bool randomizePlayersOrder = false, int fieldsCount = DefaultFieldsCount)
        {
            players = GameInitializer.GeneratePlayers(playerNames, randomizePlayersOrder).ToList();
            camels = GameInitializer.GenerateCamels().ToList();
            fields = GameInitializer.GenerateFields(fieldsCount).ToList();
            camelPositions = GameInitializer.SetCamelsOnBoard(this);
            currentPlayer = players.First();
        }

        public IActionResult DrawDice()
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

            return ActionCollector.GetActions();
        }

        public IActionResult MakeBet(Colour colour, BetType betType)
        {
            if (GameIsOver)
            {
                throw new GameOverException();
            }

            // TODO: zabranie karty graczowi
            // TODO: położenie karty na odpowiednim stosie

            SetNextPlayerAsCurrent();

            return ActionCollector.GetActions();
        }

        public IActionResult DrawTypingCard(IAvailableTypingCard card)
        {
            ITypingCard typingCard = cardManager.DrawCard(card);
            currentPlayer.AddTypingCard(typingCard);

            return ActionCollector.GetActions();
        }

        public IActionResult PlaceAudienceTile(int fieldIndex, AudienceTileSide audienceTileSide)
        { // TODO: przerobić na IAvailableField
            if (GameIsOver)
            {
                throw new GameOverException();
            }

            if (fieldIndex < 1 || fieldIndex > fields.Count)
            {
                throw new FieldNotFoundException(fieldIndex);
            }
            
            Field previousField = fields.SingleOrDefault(field => field.Index == fieldIndex - 1);
            Field nextField = fields.SingleOrDefault(field => field.Index == fieldIndex + 1);
            if (previousField?.AudienceTile != null || nextField?.AudienceTile != null)
            {
                throw new NearbyFieldOccupiedByAudienceTileException(fieldIndex);
            }

            Field playersAudienceTilePreviousField = fields.SingleOrDefault(field => field.AudienceTile?.Owner == currentPlayer);
            Field targetField = fields.Single(field => field.Index == fieldIndex);
            targetField.PutAudienceTile(currentPlayer.GetAudienceTile(audienceTileSide));
            playersAudienceTilePreviousField?.RemoveAudienceTile();
            ActionCollector.AddAction(new AudienceTilePlacementStep(targetField));

            SetNextPlayerAsCurrent();

            return ActionCollector.GetActions();
        }

        private void FinishGame()
        {
            SummarizeCurrentTurn();
            ActionCollector.AddAction(new GameOverStep(this));
        }

        private void GoToNextTurn()
        {
            RemoveAllAudienceTiles();
            dicer.Reset();
            SummarizeCurrentTurn();
            ActionCollector.AddAction(new NewTurnStep());
        }

        private void SummarizeCurrentTurn()
        {
            // TODO: podsumowanie tury
        }

        private void GiveCurrentPlayerACoin() => currentPlayer.AddCoins(Dicer.DiceDrawReward);

        private void RemoveAllAudienceTiles() => fields.ForEach(field => field.RemoveAudienceTile());

        private void SetNextPlayerAsCurrent()
        {
            int currentIndex = players.ToList().IndexOf(currentPlayer);
            int playersCount = players.Count;
            currentPlayer = players.ElementAt(++currentIndex % playersCount);
            ActionCollector.AddAction(new ChangedCurrentPlayerStep(currentPlayer));
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
            if (isMadColour && newFieldIndex <= 0 
            || !isMadColour && newFieldIndex > fields.Count)
            {
                GameIsOver = true;
                return;
            }
            ActionCollector.AddAction(new CamelsMovedStep(camels, field, field = fields[newFieldIndex - 1]));

            // additional move if camel ended move on audience tile
            AudienceTile audienceTile = (AudienceTile)field.AudienceTile;
            if (audienceTile != null)
            {
                ActionCollector.AddAction(new CamelsStoodOnAudienceTileStep(audienceTile));
                ((Player)audienceTile.Owner).AddCoins(1);
                newFieldIndex = field.Index + audienceTile.MoveValue;
                ActionCollector.AddAction(new CamelsMovedStep(camels, field, field = fields[newFieldIndex - 1], audienceTile.Side.ToStackPutType()));
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

        private bool ShouldSwitchMadColour(Colour colour)
        {
            if (AreMadCamelsOnTheSameField() && IsNearestCamelOnBackMad(colour))
            {
                ActionCollector.AddAction(new MadCamelColourSwitchedStep(MadCamelColourSwitchReason.OtherMadCamelIsDirectlyOnBackOfOtherOne));
                return true;
            }

            if (!AnyNotMadCamelsOnBack(colour) && AnyNotMadCamelsOnBack(ColourHelper.GetOppositeMadColour(colour)))
            {
                ActionCollector.AddAction(new MadCamelColourSwitchedStep(MadCamelColourSwitchReason.OnlyOneMadCamelIsCarryingNonMadCamels));
                return true;
            }

            return false;
        }

        private bool IsNearestCamelOnBackMad(Colour colour) => camelPositions[colour].Camels.TakeWhile(camel => camel.Colour == colour).LastOrDefault()?.IsMad ?? false;
        private bool AnyNotMadCamelsOnBack(Colour colour) => camelPositions[colour].Camels.TakeWhile(camel => camel.Colour == colour).Where(camel => !camel.IsMad).Any();
        private bool AreMadCamelsOnTheSameField() => camelPositions[Colour.White] == camelPositions[Colour.Black];
    }
}
