using CamelUpEngine;
using CamelUpEngine.Exceptions.CamelsExceptions;
using NUnit.Framework;
using System.Linq;

namespace TestCamelUpEngine.CamelsOnField
{
    internal class GettingCamelFromStack : BaseClass
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            field.PutCamelsOnTop(Camels.ToList());
        }

        [Test, Sequential]
        public void TestGettingFirst() => TestGettingNth(0);

        [Test, Sequential]
        public void TestGettingMiddle() => TestGettingNth(Camels.Count / 2);

        [Test, Sequential]
        public void TestGettingLast() => TestGettingNth(Camels.Count - 1);

        [Test, Sequential]
        public void TestGettingNotExisting()
        {
            Assert.Throws(Is.InstanceOf<NoCamelFoundException>(), () => field.TakeOffCamel(Colour.Violet));
        }

        private void TestGettingNth(int index)
        {
            var takenCamels = field.TakeOffCamel(Camels.ToArray()[index].Colour);
            var camelsAfterTakeOff = field.GetCamels().ToList();

            Assert.Multiple(() =>
            {
                CollectionAssert.AreEqual(Camels.Take(index + 1).GetColours(), takenCamels.GetColours());
                CollectionAssert.AreEqual(Camels.Skip(index + 1).GetColours(), camelsAfterTakeOff.GetColours());
            });
        }
    }
}