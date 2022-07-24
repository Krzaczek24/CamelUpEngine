using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions;
using CamelUpEngine.GameObjects;
using CamelUpEngine.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CamelUpEngine.GameTools
{
    internal static class GameInitializer
    {
        private static bool IsInvalidPlayerName(string playerName) => !Regex.IsMatch(playerName, @"^\w+(\s?\w+)+$");
        public static IEnumerable<Player> GeneratePlayers(IEnumerable<string> playerNames, bool randomizePlayersOrder)
        {
            int playersCount = playerNames?.Count() ?? 0;

            if (playersCount < Game.MINIMAL_PLAYERS_COUNT)
            {
                throw new TooFewPlayersException(playersCount);
            }

            if (playersCount > Game.MAXIMAL_PLAYERS_COUNT)
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

        public static IEnumerable<Camel> GenerateCamels()
        {
            foreach (Colour colour in ColourHelper.AllCamelColours)
            {
                yield return new Camel(colour);
            }
        }

        public static IEnumerable<Field> GenerateFields(int fieldsCount)
        {
            if (fieldsCount < Game.DEFAULT_FIELDS_COUNT)
            {
                throw new ArgumentException($"Number of fields cannot be lesser than {Game.DEFAULT_FIELDS_COUNT}");
            }

            for (int index = 1; index <= fieldsCount; index++)
            {
                yield return new Field(index);
            }
        }

        public static Dictionary<Colour, Field> SetCamelsOnBoard(Game game)
        {
            Dictionary<Colour, Field> camelPositions = new();

            var drawnDices = Dicer.DrawDicesForInitialCamelsPlacement();
            foreach (IDrawnDice dice in drawnDices)
            {
                Field camelField = SetCamelInitialPosition(game, camelPositions, dice.Colour, dice.Value);
                camelPositions.Add(dice.Colour, camelField);
            }

            return camelPositions;
        }

        private static Field SetCamelInitialPosition(Game game, Dictionary<Colour, Field> camelPositions, Colour camelColour, int position)
        {
            if (ColourHelper.IsMadColour(camelColour))
            {
                position = game.Fields.Count + position + 1;
            }

            if (camelPositions.TryGetValue(camelColour, out Field camelField))
            {
                throw new CamelAlreadyOnboardException(camelColour, camelField.Index);
            }

            Camel camel = game.Camels.Single(camel => camel.Colour == camelColour) as Camel;
            Field field = game.Fields.Single(field => field.Index == position) as Field;
            field.PutCamels(new[] { camel }.ToList());
            return field;
        }
    }
}
