using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions;
using CamelUpEngine.GameTools;
using CamelUpEngine.Helpers;
using NUnit.Framework;
using System.Linq;

namespace TestCamelUpEngine.Camels
{
    internal class GettingCamelFromStack
    {
        private const int FIRST = 0;
        private const int MIDDLE = 3; // CamelMoveTester.Camels.Count / 2
        private const int LAST = 5; // CamelMoveTester.Camels.Count - 1

        private readonly CamelMoveTester tester = new();

        [SetUp]
        public void SetUp()
        {
            tester.ResetField();
            tester.PutCamelsOnTop(CamelMoveTester.Camels.ToList());
        }

        [Test, Sequential]
        public void TestGettingNth([Values(FIRST, MIDDLE, LAST)] int index)
        {
            var takenCamels = tester.TakeOffCamel(CamelMoveTester.Camels.ToArray()[index].Colour);
            var camelsAfterTakeOff = tester.FieldCamels.ToList();

            Assert.Multiple(() =>
            {
                CollectionAssert.AreEqual(CamelMoveTester.Camels.Take(index + 1).GetColours(), takenCamels.GetColours());
                CollectionAssert.AreEqual(CamelMoveTester.Camels.Skip(index + 1).GetColours(), camelsAfterTakeOff.GetColours());
            });
        }

        [Test, Sequential]
        public void TestGettingNotExisting()
        {
            Assert.Throws(Is.InstanceOf<NoCamelFoundException>(), () => tester.TakeOffCamel(Colour.Violet));
        }
    }
}