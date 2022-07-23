using CamelUpEngine;
using CamelUpEngine.Core.Actions;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions.AudienceTilesExceptions;
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

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            game = new(players);
            camelsPositions = game.Fields.Where(field => field.Camels.Any()).Select(field => field.Index).ToArray();
        }

        [Test]
        public void TestPutAudienceTileOnStartField()
        {
            Assert.Throws<PuttingAudienceTileOnStartFieldException>(() => game.PlaceAudienceTile(1, AudienceTileSide.Cheering));
        }

        [Test]
        public void TestAudienceTilePlacementOnFieldOccupiedByCamel()
        {
            int nonStartFieldIndex = camelsPositions.First(i => i != 1);
            Assert.Throws<FieldOccupiedByCamelException>(() => game.PlaceAudienceTile(nonStartFieldIndex, AudienceTileSide.Cheering));
        }

        [Test]
        public void TestAudienceTilePlacementOnFieldNextToFieldOccupiedByAnotherAudienceTile()
        {
            int farFieldIndex = 7;
            game.PlaceAudienceTile(farFieldIndex, AudienceTileSide.Cheering);
            Assert.Multiple(() =>
            {
                Assert.Throws<NearbyFieldOccupiedByAudienceTileException>(() => game.PlaceAudienceTile(farFieldIndex - 1, AudienceTileSide.Cheering));
                Assert.Throws<NearbyFieldOccupiedByAudienceTileException>(() => game.PlaceAudienceTile(farFieldIndex + 1, AudienceTileSide.Cheering));
            });
        }

        [Test]
        public void TestCorrectAudienceTilesPlacement()
        {
            throw new System.NotImplementedException();
        }
    }
}
