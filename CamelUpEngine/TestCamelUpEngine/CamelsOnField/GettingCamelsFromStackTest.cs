using CamelUpEngine.Core;
using CamelUpEngine.Exceptions.CamelsExceptions;
using CamelUpEngine.GameTools;
using NUnit.Framework;
using System.Linq;

namespace TestCamelUpEngine.CamelsOnField
{
    internal class GettingCamelFromStack : BaseClass
    {
        private const int FIRST = 0;
        private const int MIDDLE = 3; // CamelMoveTester.Camels.Count / 2
        private const int LAST = 5; // CamelMoveTester.Camels.Count - 1

        [SetUp]
        public override void Setup()
        {
            base.Setup();
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