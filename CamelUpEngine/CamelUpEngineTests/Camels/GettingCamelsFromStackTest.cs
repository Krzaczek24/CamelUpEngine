#if DEBUG
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions;
using CamelUpEngine.GameObjects;
using CamelUpEngine.GameTools;
using CamelUpEngine.Helpers;
using CamelUpEngine.Helpers.TestHelpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine.Camels
{
    internal class GettingCamelFromStack
    {
        private static IReadOnlyCollection<ICamel> Camels { get; } = CamelHelper.GetCamels(Colour.Red, Colour.Green, Colour.Black, Colour.Blue, Colour.Yellow, Colour.White);

        private const int FIRST = 0;
        private const int MIDDLE = 3; // CamelMoveTester.Camels.Count / 2
        private const int LAST = 5; // CamelMoveTester.Camels.Count - 1

        private readonly CamelMoveTester tester = new();

        [SetUp]
        public void SetUp()
        {
            tester.ResetField();
            tester.PutCamelsOnTop(Camels.ToList());
        }

        [Test, Sequential]
        public void TestGettingNth([Values(FIRST, MIDDLE, LAST)] int index)
        {
            var takenCamels = tester.TakeOffCamel(Camels.ToArray()[index].Colour);
            var camelsAfterTakeOff = tester.FieldCamels.ToList();

            Assert.Multiple(() =>
            {
                CollectionAssert.AreEqual(Camels.Take(index + 1).GetColours(), takenCamels.GetColours());
                CollectionAssert.AreEqual(Camels.Skip(index + 1).GetColours(), camelsAfterTakeOff.GetColours());
            });
        }

        [Test, Sequential]
        public void TestGettingNotExisting()
        {
            Assert.Throws(Is.InstanceOf<NoCamelFoundException>(), () => tester.TakeOffCamel(Colour.Violet));
        }
    }
}
#endif