using CamelUpEngine.Exceptions.CamelsExceptions;
using CamelUpEngine.Exceptions.PlayersExceptions;
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

        private readonly ICollection<Player> players;
        private readonly ICollection<Camel> camels;
        private readonly ICollection<Field> fields;
        private readonly IDictionary<Colour, Field> camelPositions;
        private readonly Dicer dicer = new Dicer();

        public IPlayer CurrentPlayer { get; private set; }
        public IReadOnlyCollection<IPlayer> Players => players.ToList();
        public IReadOnlyCollection<ICamel> Camels => camels.ToList();
        public IReadOnlyCollection<IField> Fields => fields.ToList();

        public Game(IEnumerable<string> playerNames, bool randomizePlayersOrder = false, int fieldsCount = DEFAULT_FIELDS_COUNT)
        {
            players = GeneratePlayers(playerNames, randomizePlayersOrder).ToList();
            camels = GenerateCamels().ToList();
            fields = GenerateFields(fieldsCount).ToList();
            camelPositions = camels.ToDictionary(camel => camel.Colour, camel => (Field)null);

            SetCamelsOnBoard();
            SetNextPlayerAsCurrent();
        }

        public void RollTheDice()
        {
            IDrawnDice dice = dicer.DrawDice();

            //przesunięcie wielbłąda
            //dodanie piniążka
            //popchnięcie tury
        }

        public void BetTheWinner()
        {
            //zabranie karty graczowi
            //położenie karty na odpowiednim stosie
            //popchnięcie tury
        }

        public void BetTheLoser()
        {
            //zabranie karty graczowi
            //położenie karty na odpowiednim stosie
            //popchnięcie tury
        }

        private void SetCamelsOnBoard()
        {
            var drawnDices = Dicer.DrawDicesForInitialCamelsPlacement();
            foreach (IDrawnDice dice in drawnDices)
            {
                SetCamelInitialPosition(dice.Colour, dice.Value);
            }
        }

        private void SetNextPlayerAsCurrent()
        {
            int currentIndex = players.ToList().IndexOf((Player)CurrentPlayer);
            int playersCount = players.Count;
            CurrentPlayer = players.ElementAt(++currentIndex % playersCount);
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
            field.PutCamelsOnTop(camel);
            camelPositions[camel.Colour] = field;
        }

        private static bool IsInvalidPlayerName(string playerName) => !Regex.IsMatch(playerName, @"^\w+(\s?\w+)+$");

        //public IField GetCamelField(Colour colour)
        //{
        //    return FindCamelField(colour) ?? throw new NoCamelFoundException(colour);
        //}

        //private Field FindCamelField(Colour colour)
        //{
        //    foreach (Field field in fields)
        //    {
        //        if (field.HasCamel(colour))
        //        {
        //            return field;
        //        }
        //    }

        //    return null;
        //}
    }
}
