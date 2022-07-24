using CamelUpEngine;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine.AudienceTileOnFields
{
    internal class SettingAudienceTileOnFieldTest
    {
        protected static IReadOnlyCollection<string> players = new[] { "Bezimienny", "Diego", "Gorn", "Milten", "Lester" };
        protected Game game;
        protected int[] camelsPositions;

        [SetUp]
        public void SetUp()
        {
            game = new(players);
            camelsPositions = game.Fields.Where(field => field.Camels.Any()).Select(field => field.Index).ToArray();
        }

        [Test, Sequential]
        public void TestPutAudienceTileOnStartField()
        {
            Assert.Throws<PuttingAudienceTileOnStartFieldException>(() => game.PlaceAudienceTile(1, AudienceTileSide.Cheering));
        }

        [Test, Sequential]
        public void TestAudienceTilePlacementOnFieldOccupiedByCamel()
        {
            int nonStartFieldIndex = camelsPositions.First(i => i != 1);
            Assert.Throws<FieldOccupiedByCamelException>(() => game.PlaceAudienceTile(nonStartFieldIndex, AudienceTileSide.Cheering));
        }

        [Test, Sequential]
        public void TestAudienceTilePlacementOnFieldNextToFieldOccupiedByAnotherAudienceTile()
        {
            const int farFieldIndex = 7;
            game.PlaceAudienceTile(farFieldIndex, AudienceTileSide.Cheering);
            Assert.Multiple(() =>
            {
                Assert.Throws<NearbyFieldOccupiedByAudienceTileException>(() => game.PlaceAudienceTile(farFieldIndex - 1, AudienceTileSide.Cheering));
                Assert.Throws<NearbyFieldOccupiedByAudienceTileException>(() => game.PlaceAudienceTile(farFieldIndex + 1, AudienceTileSide.Cheering));
            });
        }

        [Test, Sequential]
        public void TestCorrectAudienceTilesPlacement()
        {
            int firstFieldIdnex = 4;
            int fieldIndex = firstFieldIdnex;

            // first player first time placing the audience tile
            game.PlaceAudienceTile(firstFieldIdnex, AudienceTileSide.Cheering);

            // rest of players and first player again placed the audience tile
            game.Players.ToList().ForEach(_ => game.PlaceAudienceTile(fieldIndex += 2, AudienceTileSide.Cheering));

            // checking if first player's previous audiencie tile is moved (not doubled)
            Assert.Multiple(() => 
            {
                Assert.IsNull(game.Fields.Single(field => field.Index == firstFieldIdnex).AudienceTile);
                Assert.DoesNotThrow(() => game.PlaceAudienceTile(firstFieldIdnex, AudienceTileSide.Cheering));
            });
        }
    }
}
