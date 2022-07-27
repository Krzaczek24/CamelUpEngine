using CamelUpEngine;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions;
using CamelUpEngine.GameObjects;
using CamelUpEngine.Helpers.TestHelpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine.AudienceTileOnFields
{
    internal class SettingAudienceTileOnFieldTest
    {
        private static IReadOnlyCollection<string> players = new[] { "Bezimienny", "Diego", "Gorn", "Milten", "Lester" };
        private IReadOnlyCollection<IAvailableField> allFields;
        private Game game;
        private int[] camelsPositions;

        [SetUp]
        public void SetUp()
        {
            game = new(players, guidGenerationFunction: () => Guid.Empty);
            camelsPositions = game.Fields.Where(field => field.Camels.Any()).Select(field => field.Index).ToArray();
            allFields = FieldHelper.GetAllFields(game);
        }

        [Test, Sequential]
        public void TestPutAudienceTileOnStartField()
        {
            IAvailableField field = allFields.Single(field => field.Index == 1);
            Assert.Throws<PuttingAudienceTileOnStartFieldException>(() => game.PlaceAudienceTile(field, AudienceTileSide.Cheering));
        }

        [Test, Sequential]
        public void TestAudienceTilePlacementOnFieldOccupiedByCamel()
        {
            int nonStartFieldIndex = camelsPositions.First(i => i != 1);
            IAvailableField field = allFields.Single(field => field.Index == nonStartFieldIndex);
            Assert.Throws<FieldOccupiedByCamelException>(() => game.PlaceAudienceTile(field, AudienceTileSide.Cheering));
        }

        [Test, Sequential]
        public void TestAudienceTilePlacementOnFieldNextToFieldOccupiedByAnotherAudienceTile()
        {
            const int farFieldIndex = 7;
            IAvailableField previousField = allFields.Single(field => field.Index == farFieldIndex - 1);
            IAvailableField currentField = allFields.Single(field => field.Index == farFieldIndex);
            IAvailableField nextField = allFields.Single(field => field.Index == farFieldIndex + 1);
            game.PlaceAudienceTile(currentField, AudienceTileSide.Cheering);
            Assert.Multiple(() =>
            {
                Assert.Throws<NearbyFieldOccupiedByAudienceTileException>(() => game.PlaceAudienceTile(previousField, AudienceTileSide.Cheering));
                Assert.Throws<NearbyFieldOccupiedByAudienceTileException>(() => game.PlaceAudienceTile(nextField, AudienceTileSide.Cheering));
            });
        }

        [Test, Sequential]
        public void TestCorrectAudienceTilesPlacement()
        {
            int fieldIndex = 4;
            Func<IAvailableField> getNextField = () =>
            {
                IAvailableField field = allFields.Single(field => field.Index == fieldIndex);
                fieldIndex += 2;
                return field;
            };
            IAvailableField firstField = getNextField();

            // first player first time placing the audience tile
            game.PlaceAudienceTile(firstField, AudienceTileSide.Cheering);

            // rest of players and first player again placed the audience tile
            game.Players.ToList().ForEach(_ => game.PlaceAudienceTile(getNextField(), AudienceTileSide.Cheering));

            // checking if first player's previous audiencie tile is moved (not doubled)
            Assert.Multiple(() => 
            {
                Assert.IsNull(game.Fields.Single(field => field.Index == firstField.Index).AudienceTile);
                Assert.DoesNotThrow(() => game.PlaceAudienceTile(firstField, AudienceTileSide.Cheering));
            });
        }
    }
}
