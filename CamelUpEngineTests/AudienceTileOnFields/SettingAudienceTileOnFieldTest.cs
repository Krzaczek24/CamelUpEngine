#if DEBUG

using CamelUpEngine;
using CamelUpEngine.Core.Actions;
using CamelUpEngine.Core.Actions.Events;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions;
using CamelUpEngine.GameObjects;
using CamelUpEngine.GameObjects.Available;
using CamelUpEngine.GameTools;
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
            game = new(players);
            camelsPositions = game.Fields.Where(field => field.Camels.Any()).Select(field => field.Index).ToArray();
            allFields = FieldHelper.GetAllFields(game);
        }

        [Test,Sequential]
        public void TestPutAudienceTileOnUnavailableField()
        {
            IAvailableField field = allFields.First();
            Assert.Throws<FieldExpiredAvailabilityException>(() => game.PlaceAudienceTile(field, AudienceTileSide.Cheering));
        }

        [Test, Sequential]
        public void TestPutAudienceTileOnStartField()
        {
            IAvailableField field = allFields.First();
            Assert.Throws<PuttingAudienceTileOnStartFieldException>(() => TestPlaceAudienceTile(field));
        }

        [Test, Sequential]
        public void TestAudienceTilePlacementOnFieldOccupiedByCamel()
        {
            int nonStartFieldIndex = camelsPositions.First(i => i != 1);
            IAvailableField field = allFields.Single(field => field.Index == nonStartFieldIndex);
            Assert.Throws<FieldOccupiedByCamelException>(() => TestPlaceAudienceTile(field));
        }

        [Test, Sequential]
        public void TestAudienceTilePlacementOnFieldNextToFieldOccupiedByAnotherAudienceTile()
        {
            const int farFieldIndex = 7;
            IAvailableField previousField = allFields.Single(field => field.Index == farFieldIndex - 1);
            IAvailableField currentField = allFields.Single(field => field.Index == farFieldIndex );
            IAvailableField nextField = allFields.Single(field => field.Index == farFieldIndex + 1);
            PlaceAudienceTileForReal(currentField);
            Assert.Multiple(() =>
            {
                Assert.Throws<NearbyFieldOccupiedByAudienceTileException>(() => TestPlaceAudienceTile(previousField));
                Assert.Throws<NearbyFieldOccupiedByAudienceTileException>(() => TestPlaceAudienceTile(nextField));
            });
        }

        [Test, Sequential]
        public void TestCorrectAudienceTilesPlacement()
        {
            IAvailableField fourthField, nextField = null;
            fourthField = allFields.Single(field => field.Index == 4);

            // first player first time placing the audience tile
            PlaceAudienceTileForReal(fourthField);

            // rest of players and first player again placed the audience tile
            foreach (var _ in game.Players)
            {
                nextField = allFields.Single(field => field.Index == (nextField ?? fourthField).Index + 2);
                PlaceAudienceTileForReal(nextField);
            }

            // checking if first player's previous audiencie tile is moved (not doubled)
            Assert.IsNull(game.Fields.Single(field => field.Index == fourthField.Index).AudienceTile);

            for (int fieldIndex = 6, playerIndex = 1; fieldIndex <= 14; fieldIndex += 2, playerIndex = (playerIndex + 1) % game.Players.Count())
            {
                IField nthField = game.Fields.Single(field => field.Index == fieldIndex);
                Assert.IsNotNull(nthField.AudienceTile);
                Assert.AreEqual(nthField.AudienceTile.Owner, game.Players.ElementAt(playerIndex));
            }
        }

        private ActionEvents PlaceAudienceTileForReal(IAvailableField targetField, AudienceTileSide tileSide = AudienceTileSide.Cheering) => 
            game.PlaceAudienceTile(game.AudienceTileAvailableFields.Single(field => field.Index == targetField.Index), tileSide);

        private IAudienceTilePlacementEvent TestPlaceAudienceTile(IAvailableField targetField, AudienceTileSide tileSide = AudienceTileSide.Cheering) =>
            AudienceTilesManager.TestPlaceAudienceTile(game.CurrentPlayer, game.Fields, targetField, tileSide, out _, () => Guid.Empty);
    }
}

#endif