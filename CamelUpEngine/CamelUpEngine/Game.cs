using CamelUpEngine.Core;
using CamelUpEngine.Exceptions.CamelsExceptions;
using CamelUpEngine.Exceptions.PlayersExceptions;
using CamelUpEngine.GameObjects;
using CamelUpEngine.GameTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        public bool GameOver { get; private set; }
        public bool TurnIsOver => dicer.DrawnDices.Count() >= MAXIMAL_DRAWN_DICES;

        public Game(IEnumerable<string> playerNames, bool randomizePlayersOrder = false, int fieldsCount = DEFAULT_FIELDS_COUNT)
        {
            players = GeneratePlayers(playerNames, randomizePlayersOrder).ToList();
            camels = GenerateCamels().ToList();
            fields = GenerateFields(fieldsCount).ToList();
            camelPositions = camels.ToDictionary(camel => camel.Colour, camel => (Field)null);

            SetCamelsOnBoard();
            SetNextPlayerAsCurrent();
        }

        public IActionResult DrawTheDice()
        {
            ((Player)CurrentPlayer).AddCoins(Dicer.DICE_DRAW_REWARD);

            IDrawnDice dice = dicer.DrawDice();

            MoveCamel(dice.Colour, dice.Value);
            //dodanie pieniążka jeśli
            SetNextPlayerAsCurrent();

            if (TurnIsOver)
            {
                GoToNextTurn();
            }

            return new ActionSuccess();
        }

        public IActionResult BetTheWinner()
        {
            //zabranie karty graczowi
            //położenie karty na odpowiednim stosie
            SetNextPlayerAsCurrent();

            return new ActionSuccess();
        }

        public IActionResult BetTheLoser()
        {
            //zabranie karty graczowi
            //położenie karty na odpowiednim stosie
            SetNextPlayerAsCurrent();

            return new ActionSuccess();
        }

        public IActionResult PlaceAudienceTile()
        {
            //zabranie kafelka z planszy jeśli był
            //sprawdzenie sąsiednich pól
            //położenie kafelka
            SetNextPlayerAsCurrent();

            return new ActionSuccess();
        }

        private void MoveCamel(Colour colour, int value)
        {
            bool isMadColour = ColourHelper.IsMadColour(colour);
            if (isMadColour)
            {
                //jeśli ten szalony niczego nie wiezie, ale za to drugi wiezie to rusz tamtym
                //jeśli ten szalony bezpośrednio nad sobą ma drugiego szalonego to rusz tym drugim
            }

            Field field = camelPositions[colour];
            List<Camel> camels = field.TakeOffCamel(colour);
            int newFieldIndex = field.Index + value - 1;
            if (isMadColour && newFieldIndex < 0 || !isMadColour && newFieldIndex >= fields.Count)
            {
                //koniec gry
            }
            field = fields[newFieldIndex];
            AudienceTile audienceTile = (AudienceTile)field.AudienceTile;
            if (audienceTile != null)
            {
                ((Player)audienceTile.Owner).AddCoins(1);
                newFieldIndex = field.Index + audienceTile.MoveValue - 1;
                field = fields[newFieldIndex];
                if (audienceTile.Side == AudienceTileSide.Booing)
                {
                    field.PutCamelsOnBottom(camels);
                    camelPositions[colour] = field;
                    return;
                }
            }

            field.PutCamelsOnTop(camels);
            camelPositions[colour] = field;
        }

        private void GoToNextTurn()
        {
            RemoveAllAudienceTiles();
            dicer.Reset();
            //podsumowanie tury - rozdanie żetonów
        }

        private void RemoveAllAudienceTiles()
        {
            fields.ForEach(field => field.RemoveAudienceTile());
        }

        private void SetNextPlayerAsCurrent()
        {
            int currentIndex = players.ToList().IndexOf((Player)CurrentPlayer);
            int playersCount = players.Count;
            CurrentPlayer = players.ElementAt(++currentIndex % playersCount);
        }

        private void SetCamelsOnBoard()
        {
            var drawnDices = Dicer.DrawDicesForInitialCamelsPlacement();
            foreach (IDrawnDice dice in drawnDices)
            {
                SetCamelInitialPosition(dice.Colour, dice.Value);
            }
        }

        private static IEnumerable<Player> GeneratePlayers(IEnumerable<string> playerNames, bool randomizePlayersOrder)
        {
            int playersCount = playerNames?.Count() ?? 0;

            if (playersCount < MINIMAL_PLAYERS_COUNT)
            {
                throw new TooFewPlayersException(playersCount);
            }

            if (playersCount > MAXIMAL_PLAYERS_COUNT)
            {
                throw new TooManyPlayersException(playersCount);
            }

            var distinctPlayerNames = playerNames.Select(name => name.ToUpper()).Distinct();
            if (distinctPlayerNames.Count() != playersCount)
            {
                throw new NotUniquePlayersNameException();
            }

            if (playerNames.Any(IsInvalidPlayerName))
            {
                throw new InvalidPlayerNameException(playerNames.First(IsInvalidPlayerName));
            }

            if (randomizePlayersOrder)
            {
                playerNames = playerNames.OrderBy(x => Guid.NewGuid()).ToList();
            }

            foreach (string playerName in playerNames)
            {
                yield return new Player(playerName);
            }
        }

        private static IEnumerable<Camel> GenerateCamels()
        {
            foreach (Colour colour in ColourHelper.AllCamelColours)
            {
                yield return new Camel(colour);
            }
        }

        private static IEnumerable<Field> GenerateFields(int fieldsCount)
        {
            if (fieldsCount < DEFAULT_FIELDS_COUNT)
            {
                throw new ArgumentException($"Number of fields cannot be lesser than {DEFAULT_FIELDS_COUNT}");
            }

            for (int index = 1; index <= fieldsCount; index++)
            {
                yield return new Field(index);
            }
        }

        private void SetCamelInitialPosition(Colour camelColour, int position)
        {
            if (ColourHelper.IsMadColour(camelColour))
            {
                position = fields.Count + position + 1;
            }

            Field field = camelPositions[camelColour];
            if (field != null)
            {
                throw new CamelAlreadyOnboardException(camelColour, field.Index);
            }

            var camel = camels.Single(camel => camel.Colour == camelColour);
            field = fields.Single(field => field.Index == position);
            field.PutCamelsOnTop(new[] { camel }.ToList());
            camelPositions[camel.Colour] = field;
        }

        private static bool IsInvalidPlayerName(string playerName) => !Regex.IsMatch(playerName, @"^\w+(\s?\w+)+$");
    }
}
