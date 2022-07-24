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
        public const int MINIMAL_PLAYERS_COUNT = 3;
        public const int MAXIMAL_PLAYERS_COUNT = 8;
        public const int DEFAULT_FIELDS_COUNT = 16;
        public const int MAXIMAL_DRAWN_DICES = 5;

        private readonly List<Player> players;
        private readonly List<Camel> camels;
        private readonly List<Field> fields;
        private readonly Dictionary<Colour, Field> camelPositions;
        private readonly Dicer dicer = new Dicer();        

        public IPlayer CurrentPlayer { get; private set; }
        public IReadOnlyCollection<IPlayer> Players => players.ToList();
        public IReadOnlyCollection<ICamel> Camels => camels.ToList();
        public IReadOnlyCollection<IField> Fields => fields.ToList();
        public IReadOnlyCollection<IDrawnDice> DrawnDices => dicer.DrawnDices;
        public IReadOnlyDictionary<Colour, int> CamelPositions => camelPositions.ToDictionary(entry => entry.Key, entry => entry.Value.Index);

        public bool GameIsOver { get; private set; }
        public bool TurnIsOver => dicer.DrawnDices.Count() >= MAXIMAL_DRAWN_DICES;

        public Game(IEnumerable<string> playerNames, bool randomizePlayersOrder = false, int fieldsCount = DEFAULT_FIELDS_COUNT)
        {
            players = GameInitializer.GeneratePlayers(playerNames, randomizePlayersOrder).ToList();
            camels = GameInitializer.GenerateCamels().ToList();
            fields = GameInitializer.GenerateFields(fieldsCount).ToList();
            camelPositions = GameInitializer.SetCamelsOnBoard(this);
            CurrentPlayer = players.First();
        }

        public IActionResult DrawTheDice()
        {
            if (GameIsOver)
            {
                throw new GameOverException();
            }

            GiveCurrentPlayerACoin();

            IDrawnDice dice = dicer.DrawDice();
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

        public IActionResult BetTheWinner()
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

        public IActionResult BetTheLoser()
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

        public IActionResult PlaceAudienceTile(int fieldIndex, AudienceTileSide audienceTileSide)
        {
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

            Field playersAudienceTilePreviousField = fields.SingleOrDefault(field => field.AudienceTile?.Owner == CurrentPlayer);
            Field targetField = fields.Single(field => field.Index == fieldIndex);
            targetField.PutAudienceTile(((Player)CurrentPlayer).GetAudienceTile(audienceTileSide));
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

        private void GiveCurrentPlayerACoin() => ((Player)CurrentPlayer).AddCoins(Dicer.DICE_DRAW_REWARD);

        private void RemoveAllAudienceTiles() => fields.ForEach(field => field.RemoveAudienceTile());

        private bool AreMadCamelsOnTheSameField() => camelPositions[Colour.White] == camelPositions[Colour.Black];

        private void SetNextPlayerAsCurrent()
        {
            int currentIndex = players.ToList().IndexOf((Player)CurrentPlayer);
            int playersCount = players.Count;
            CurrentPlayer = players.ElementAt(++currentIndex % playersCount);
            ActionCollector.AddAction(new ChangedCurrentPlayerStep(CurrentPlayer));
        }

        private void MoveCamel(Colour colour, int value)
        {
            bool isMadColour = ColourHelper.IsMadColour(colour);
            if (isMadColour)
            {
                // TODO: jeśli ten szalony niczego nie wiezie, ale za to drugi wiezie to rusz tamtym
                // TODO: jeśli ten szalony bezpośrednio nad sobą ma drugiego szalonego to rusz tym drugim
                if (AreMadCamelsOnTheSameField())
                {

                }
            }

            Field field = camelPositions[colour];
            List<Camel> camels = field.TakeOffCamel(colour);
            int newFieldIndex = field.Index + value - 1;
            if (isMadColour && newFieldIndex < 0 || !isMadColour && newFieldIndex >= fields.Count)
            {
                GameIsOver = true;
                return;
            }
            ActionCollector.AddAction(new CamelsMovedStep(camels, field, field = fields[newFieldIndex]));
            AudienceTile audienceTile = (AudienceTile)field.AudienceTile;
            if (audienceTile != null)
            {
                ActionCollector.AddAction(new CamelsStoodOnAudienceTileStep(audienceTile));
                ((Player)audienceTile.Owner).AddCoins(1);
                newFieldIndex = field.Index + audienceTile.MoveValue - 1;
                ActionCollector.AddAction(new CamelsMovedStep(camels, field, field = fields[newFieldIndex], audienceTile.Side.ToStackPutType()));
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
    }
}
