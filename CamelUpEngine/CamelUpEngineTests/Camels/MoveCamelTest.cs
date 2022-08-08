using CamelUpEngine;
using CamelUpEngine.Core.Actions;
using CamelUpEngine.Core.Actions.Events;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions;
using CamelUpEngine.Extensions;
using CamelUpEngine.GameObjects;
using CamelUpEngine.GameObjects.Available;
using CamelUpEngine.GameTools;
using CamelUpEngine.Helpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine.Camels
{
    internal class MoveCamelTest
    {
        private const int FIRST = 0;
        private const int MIDDLE = 3; // CamelMoveTester.Camels.Count / 2
        private const int LAST = 6; // CamelMoveTester.Camels.Count - 1
        private const int MEETUP_FIELD_INDEX = 10;
        private static IReadOnlyCollection<string> players = new[] { "Bezimienny", "Diego", "Gorn", "Milten", "Lester" };
        private Game game;
        private CamelTrafficManager manager;

        [SetUp]
        public void SetUp()
        {
            game = new Game(players);
            manager = new(game.Fields);
        }

        [Test, Sequential, Repeat(1000)]
        public void TestCamelSingleMove()
        {
            var dicer = new Dicer();

            for (int moves = 10; moves > 0; moves--)
            {
                TestCamelSingleMove(dicer);
            }
        }

        [Test, Sequential, Repeat(1000)]
        public void TestStackMove()
        {
            MoveAllCamelsOnDifferentField();
            var camelColours = manager.OrderedAllCamels.Reverse().GetColours().ToList();
            var camels = new Dictionary<char, Colour>()
            {
                ['A'] = camelColours.ElementAt(0),
                ['B'] = camelColours.ElementAt(1),
                ['C'] = camelColours.ElementAt(2),
                ['D'] = camelColours.ElementAt(3),
                ['E'] = camelColours.ElementAt(4),
                ['X'] = camelColours.ElementAt(5),
                ['Y'] = camelColours.ElementAt(6),
            };
            // (A) B C D E X Y [+1]
            TestStackMove(camels['A'], 1, 2, 0, 2);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 1).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 2).Value, new[] { camels['A'], camels['B'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 3).Value, new[] { camels['C'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 4).Value, new[] { camels['D'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 5).Value, new[] { camels['E'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 6).Value, new[] { camels['X'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 7).Value, new[] { camels['Y'] });
            //    A
            // _ (B) C D E X Y [+1]
            TestStackMove(camels['B'], 1, 3, 0, 3);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 1).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 2).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 3).Value, new[] { camels['A'], camels['B'], camels['C'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 4).Value, new[] { camels['D'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 5).Value, new[] { camels['E'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 6).Value, new[] { camels['X'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 7).Value, new[] { camels['Y'] });
            //      A
            //     (B)
            // _ _  C  D E X Y [+1]
            TestStackMove(camels['B'], 1, 4, 1, 3);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 1).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 2).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 3).Value, new[] { camels['C'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 4).Value, new[] { camels['A'], camels['B'], camels['D'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 5).Value, new[] { camels['E'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 6).Value, new[] { camels['X'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 7).Value, new[] { camels['Y'] });
            //         A
            //         B
            // _ _  C  D E X (Y) [-1]
            TestStackMove(camels['Y'], -1, 6, 0, 2);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 1).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 2).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 3).Value, new[] { camels['C'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 4).Value, new[] { camels['A'], camels['B'], camels['D'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 5).Value, new[] { camels['E'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 6).Value, new[] { camels['Y'], camels['X'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 7).Value, new Colour[] { });
            //         A
            //         B    Y
            // _ _  C  D E (X) [-1]
            TestStackMove(camels['X'], -1, 6, 1, 1);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 1).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 2).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 3).Value, new[] { camels['C'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 4).Value, new[] { camels['A'], camels['B'], camels['D'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 5).Value, new[] { camels['Y'], camels['E'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 6).Value, new[] { camels['X'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 7).Value, new Colour[] { });
            //         A
            //         B Y  
            // _ _ (C) D E X [+2]
            TestStackMove(camels['C'], 2, 5, 0, 3);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 1).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 2).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 3).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 4).Value, new[] { camels['A'], camels['B'], camels['D'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 5).Value, new[] { camels['C'], camels['Y'], camels['E'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 6).Value, new[] { camels['X'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 7).Value, new Colour[] { });
            //       A C
            //       B Y  
            // _ _ _ D E (X) [-3]
            TestStackMove(camels['X'], -3, 6, 1, 1);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 1).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 2).Value, new[] { camels['C'], camels['Y'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 3).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 4).Value, new[] { camels['A'], camels['B'], camels['D'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 5).Value, new[] { camels['E'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 6).Value, new[] { camels['X'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 7).Value, new Colour[] { });
            //       (A)
            //   C    B
            // _ Y _  D E X [+1]
            TestStackMove(camels['A'], 1, 5, 2, 2);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 1).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 2).Value, new[] { camels['C'], camels['Y'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 3).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 4).Value, new[] { camels['B'], camels['D'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 5).Value, new[] { camels['A'], camels['E'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 6).Value, new[] { camels['X'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 7).Value, new Colour[] { });
            //   C   B (A)
            // _ Y _ D  E X [+1]
            TestStackMove(camels['A'], 1, 6, 1, 2);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 1).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 2).Value, new[] { camels['C'], camels['Y'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 3).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 4).Value, new[] { camels['B'], camels['D'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 5).Value, new[] { camels['E'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 6).Value, new[] { camels['A'], camels['X'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 7).Value, new Colour[] { });
            //   C   B   (A)
            // _ Y _ D  E X [+1]
            TestStackMove(camels['A'], 1, 7, 1, 1);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 1).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 2).Value, new[] { camels['C'], camels['Y'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 3).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 4).Value, new[] { camels['B'], camels['D'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 5).Value, new[] { camels['E'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 6).Value, new[] { camels['X'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 7).Value, new[] { camels['A'] });
            //   C   B
            // _ Y _ D E X A
        }

        [Test, Sequential, Repeat(1000)]
        public void TestSteppingOnAudienceTile()
        {
            const int booingTileFieldIndex = 10;
            const int cheeringTileFieldIndex = 12;

            IAvailableField field = game.AudienceTileAvailableFields.Single(field => field.Index == booingTileFieldIndex);
            game.PlaceAudienceTile(field, AudienceTileSide.Booing);

            //Assert.Throws<FieldExpiredAvailabilityException>(() => game.PlaceAudienceTile(field, AudienceTileSide.Cheering));

            field = game.AudienceTileAvailableFields.Single(field => field.Index == cheeringTileFieldIndex);
            game.PlaceAudienceTile(field, AudienceTileSide.Cheering);

            manager = new(game.Fields);
            MoveAllCamelsOnDifferentField();
            var camelColours = manager.OrderedAllCamels.Reverse().GetColours().ToList();
            var camels = new Dictionary<char, Colour>()
            {
                ['A'] = camelColours.ElementAt(0),
                ['B'] = camelColours.ElementAt(1),
                ['C'] = camelColours.ElementAt(2),
                ['D'] = camelColours.ElementAt(3),
                ['E'] = camelColours.ElementAt(4),
                ['X'] = camelColours.ElementAt(5),
                ['Y'] = camelColours.ElementAt(6),
            };

            TestStackMove(camels['A'], 7, 8, 0, 1);
            TestStackMove(camels['B'], 6, 8, 0, 2);
            TestStackMove(camels['C'], 6, 9, 0, 1);
            TestStackMove(camels['D'], 5, 9, 0, 2);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 8).Value, new[] { camels['B'], camels['A'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 9).Value, new[] { camels['D'], camels['C'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 10).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 11).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 12).Value, new Colour[] { });
            // B (D)
            // A  C  - _ + [+1]
            TestStackMove(camels['D'], 1, 9, 2, 2);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 8).Value, new[] { camels['B'], camels['A'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 9).Value, new[] { camels['C'], camels['D'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 10).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 11).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 12).Value, new Colour[] { });
            // B  C
            // A (D) - _ + [+1]
            TestStackMove(camels['D'], 1, 9, 2, 2);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 8).Value, new[] { camels['B'], camels['A'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 9).Value, new[] { camels['C'], camels['D'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 10).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 11).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 12).Value, new Colour[] { });
            //  B  C
            // (A) D - _ + [+2]
            TestStackMove(camels['A'], 2, 9, 0, 4);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 8).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 9).Value, new[] { camels['C'], camels['D'], camels['B'], camels['A'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 10).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 11).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 12).Value, new Colour[] { });
            //   (C)
            //    D
            //    B
            // _  A - _ + [+1]
            TestStackMove(camels['C'], 1, 9, 4, 4);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 8).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 9).Value, new[] { camels['D'], camels['B'], camels['A'], camels['C'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 10).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 11).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 12).Value, new Colour[] { });
            //    D
            //   (B)
            //    A
            // _  C - _ + [+3]
            TestStackMove(camels['B'], 3, 13, 2, 2);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 8).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 9).Value, new[] { camels['A'], camels['C'] });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 10).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 11).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 12).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 13).Value, new[] { camels['D'], camels['B'] });
            //    A        D
            // _ (C) - _ + B [+3]
            TestStackMove(camels['C'], 3, 13, 0, 4);
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 8).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 9).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 10).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 11).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 12).Value, new Colour[] { });
            CollectionAssert.AreEqual(manager.Fields.ToDictionary(field => field.Index, field => field.Camels.Select(camel => camel.Colour)).Single(field => field.Key == 13).Value, new[] { camels['A'], camels['C'], camels['D'], camels['B'] });
            //           A
            //           C
            //           D
            // _ _ - _ + B
        }

        private void TestStackMove(Colour camelToMoveColour, int moveValue, int expectedCamelField, int camelPrevFieldExpectedCount, int camelCurrentFieldExpectedCount)
        {
            int camelFieldIndex = manager.CamelPositions[camelToMoveColour];
            manager.MoveCamel(camelToMoveColour, moveValue);
            Assert.That(manager.CamelPositions[camelToMoveColour], Is.EqualTo(expectedCamelField));
            Assert.That(manager.Fields.Single(field => field.Index == camelFieldIndex).Camels, Has.Count.EqualTo(camelPrevFieldExpectedCount));
            Assert.That(manager.Fields.Single(field => field.Index == expectedCamelField).Camels, Has.Count.EqualTo(camelCurrentFieldExpectedCount));
        }

        private void TestCamelSingleMove(Dicer dicer)
        {
            if (dicer.IsEmpty)
            {
                dicer.Reset();
            }

            var camelsInitialFieldIndexes = manager.CamelPositions;
            var drawnDice = dicer.DrawDice();

            var events = manager.MoveCamel(drawnDice.Colour, drawnDice.Value);
            var switchEvent = events.GetEvent<IMadCamelColourSwitchedEvent>();
            Colour currentColour = switchEvent == null ? drawnDice.Colour : ColourHelper.GetOppositeMadColour(drawnDice.Colour);

            int fieldIndexShift = drawnDice.Value;
            int camelInitialFieldIndex = camelsInitialFieldIndexes[currentColour];
            int camelActualFieldIndex = manager.CamelPositions[currentColour];

            Assert.AreEqual(camelInitialFieldIndex + fieldIndexShift, camelActualFieldIndex);
        }

        [Test, Sequential, Repeat(1000)]
        public void TestMovingNth([Values(FIRST, MIDDLE, LAST)] int index)
        {
            MoveAllCamelsOnTheSameField(MEETUP_FIELD_INDEX);
            const int shift = 1;

            ICamel camel = manager.OrderedAllCamels.ElementAt(index);
            var initialPositions = manager.CamelPositions;
            manager.MoveCamel(camel.Colour, shift);
            var newPositions = manager.CamelPositions;

            CollectionAssert.AreEqual(initialPositions.Select(p => p.Key), newPositions.Select(p => p.Key));
            Assert.That(initialPositions.Select(p => p.Value), Is.All.EqualTo(MEETUP_FIELD_INDEX));

            var movedCamelColours = manager.OrderedAllCamels.TakeUntil(c => c == camel, inclusive: true).Select(c => c.Colour).ToList();
            var notMovedCamelColours = manager.OrderedAllCamels.Select(c => c.Colour).Except(movedCamelColours).ToList();

            Assert.That(movedCamelColours, Has.Count.EqualTo(index + 1));
            Assert.That(notMovedCamelColours, Has.Count.EqualTo(manager.OrderedAllCamels.Count() - (index + 1)));

            var movedCamelPositions = newPositions.Where(p => movedCamelColours.Contains(p.Key)).Select(p => p.Value).ToList();
            var notMovedCamelPositions = newPositions.Where(p => notMovedCamelColours.Contains(p.Key)).Select(p => p.Value).ToList();

            Assert.That(movedCamelPositions, Is.All.EqualTo(MEETUP_FIELD_INDEX + shift));
            Assert.That(notMovedCamelPositions, Is.All.EqualTo(MEETUP_FIELD_INDEX));
        }

        private void MoveCamelsOnField(int fieldIndex, params ICamel[] camels)
        {
            IActionEvent switchEvent = null;
            var colours = camels.GetColours();
            foreach (Colour colour in colours)
            {
                Colour currentColour = switchEvent == null ? colour : ColourHelper.GetOppositeMadColour(colour);
                int difference = fieldIndex - manager.CamelPositions[currentColour];
                var events = manager.MoveCamel(currentColour, difference);
                switchEvent = events.GetEvent<IMadCamelColourSwitchedEvent>();
            }
        }

        private void MoveAllCamelsOnTheSameField(int fieldIndex)
        {
            IActionEvent switchEvent = null;
            foreach (Colour colour in ColourHelper.AllCamelColours)
            {
                Colour currentColour = switchEvent == null ? colour : ColourHelper.GetOppositeMadColour(colour);
                int difference = fieldIndex - manager.CamelPositions[currentColour];
                var events = manager.MoveCamel(currentColour, difference);
                switchEvent = events.GetEvent<IMadCamelColourSwitchedEvent>();
            }
        }

        private void MoveAllCamelsOnDifferentField()
        {
            var fields = manager.Fields.Take(ColourHelper.AllCardColours.Count());
            foreach (IField field in fields)
            {
                if (!field.Camels.Any())
                {
                    ICamel topCamel = manager.Fields.SkipWhile(field => field.Camels.Count() <= 1).First().Camels.First();
                    int sourceIndex = manager.CamelPositions[topCamel.Colour];
                    manager.MoveCamel(topCamel.Colour, field.Index - sourceIndex);
                }
            }
            fields = manager.Fields.Except(fields).Take(ColourHelper.MadColours.Count());
            foreach (IField field in fields)
            {
                if (!field.Camels.Any())
                {
                    ICamel topCamel = manager.Fields.Reverse().SkipWhile(field => field.Camels.Count() == 0).First().Camels.First();
                    int sourceIndex = manager.CamelPositions[topCamel.Colour];
                    manager.MoveCamel(topCamel.Colour, field.Index - sourceIndex);
                }
            }

            fields = manager.Fields.Take(ColourHelper.AllCamelColours.Count());
            var camelCount = fields.Select(field => field.Camels.Count());
            Assert.That(camelCount, Is.All.EqualTo(1));
            Assert.That(manager.Fields.Sum(field => field.Camels.Count()), Is.EqualTo(ColourHelper.AllCamelColours.Count()));
        }
    }
}
