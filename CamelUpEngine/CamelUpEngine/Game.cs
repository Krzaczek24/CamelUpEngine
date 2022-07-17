using CamelUpEngine.Exceptions.CamelsExceptions;
using CamelUpEngine.Exceptions.PlayersExceptions;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private Dicer Dicer { get; } = new Dicer();
        public IPlayer CurrentPlayer { get; }

        public IReadOnlyCollection<Player> Players => players.ToList();
        public IReadOnlyCollection<ICamel> Camels => camels.ToList();
        public IReadOnlyCollection<IField> Fields => fields.ToList();

        public Game(IEnumerable<string> playerNames, bool randomizePlayersOrder = false, int fieldsCount = DEFAULT_FIELDS_COUNT)
        {
            if (randomizePlayersOrder)
            {
                playerNames = playerNames.OrderBy(x => Guid.NewGuid()).ToList();
            }

            players = GeneratePlayers(playerNames).ToList();
            camels = GenerateCamels().ToList();
            fields = GenerateFields(fieldsCount).ToList();
            SetCamelsOnBoard();
        }

        public void RollTheDice()
        {
            //rzut kostką
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
            foreach (DrawnDice dice in drawnDices)
            {
                SetCamelInitialPosition(dice.Colour, dice.Value);
            }
        }

        private static IEnumerable<Player> GeneratePlayers(IEnumerable<string> playerNames)
        {
            int playersCount = playerNames.Count();

            if (playerNames == null || playersCount < MINIMAL_PLAYERS_COUNT)
            {
                throw new TooFewPlayersException(playersCount);
            }

            if (playersCount > MAXIMAL_PLAYERS_COUNT)
            {
                throw new TooManyPlayersException(playersCount);
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

            Field field = FindCamelField(camelColour);
            if (field != null)
            {
                throw new CamelAlreadyOnboardException(camelColour, field.Index);
            }

            var camel = camels.Where(camel => camel.Colour == camelColour).ToList();
            field = fields.Single(field => field.Index == position);
            field.PutCamelsOnTop(camel);
        }

        public IField GetCamelField(Colour colour)
        {
            return FindCamelField(colour) ?? throw new NoCamelFoundException(colour);
        }

        private Field FindCamelField(Colour colour)
        {
            foreach (Field field in fields)
            {
                if (field.HasCamel(colour))
                {
                    return field;
                }
            }

            return null;
        }
    }
}
