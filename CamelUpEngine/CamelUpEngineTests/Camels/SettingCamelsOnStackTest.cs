#if DEBUG
using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects;
using CamelUpEngine.GameTools;
using CamelUpEngine.Helpers.TestHelpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine.Camels
{
    internal class SettingCamelsOnStackTest
    {
        private static IReadOnlyCollection<ICamel> Camels { get; } = CamelHelper.GetCamels(Colour.Red, Colour.Green, Colour.Black, Colour.Blue, Colour.Yellow, Colour.White);
        private static IReadOnlyCollection<ICamel> CamelsFirstPack { get; } = Camels.Take(3).ToList();
        private static IReadOnlyCollection<ICamel> CamelsSecondPack { get; } = Camels.TakeLast(3).ToList();
        private static IReadOnlyCollection<ICamel> CamelsBothPacks { get; } = CamelsSecondPack.Concat(CamelsFirstPack).ToList();

        private readonly CamelMoveTester tester = new();

        [SetUp]
        public void SetUp()
        {
            tester.ResetField();
        }

        #region Field top

        [Test, Sequential]
        public void TestSettingSingleCamelsOnFieldTop()
        {
            IList<ICamel> addedCamels = new List<ICamel>();
            CollectionAssert.IsEmpty(tester.FieldCamels);
            Camels.ToList().ForEach(camel => {
                tester.PutCamelsOnTop(new[] { camel }.ToList());
                addedCamels.Insert(0, camel);
                CollectionAssert.AreEqual(addedCamels, tester.FieldCamels);
            });
        }

        [Test, Sequential]
        public void TestSettingMultipleCamelsOnFieldTop()
        {
            tester.PutCamelsOnTop(Camels.ToList());
            CollectionAssert.AreEqual(Camels, tester.FieldCamels);
        }

        [Test, Sequential]
        public void TestSettingPacksOfCamelsOnFieldTop()
        {
            tester.PutCamelsOnTop(CamelsFirstPack.ToList());
            CollectionAssert.AreEqual(CamelsFirstPack, tester.FieldCamels);

            tester.PutCamelsOnTop(CamelsSecondPack.ToList());
            CollectionAssert.AreEqual(CamelsBothPacks, tester.FieldCamels);
        }

        #endregion Field top

        #region Field bottom

        [Test, Sequential]
        public void TestSettingSingleCamelsOnFieldBottom()
        {
            IList<ICamel> addedCamels = new List<ICamel>();
            CollectionAssert.IsEmpty(tester.FieldCamels);
            Camels.ToList().ForEach(camel => {
                tester.PutCamelsOnBottom(new[] { camel }.ToList());
                addedCamels.Add(camel);
                CollectionAssert.AreEqual(addedCamels, tester.FieldCamels);
            });
        }

        [Test, Sequential]
        public void TestSettingMultipleCamelsOnFieldBottom()
        {
            tester.PutCamelsOnBottom(Camels.ToList());
            CollectionAssert.AreEqual(Camels, tester.FieldCamels);
        }

        [Test, Sequential]
        public void TestSettingPacksOfCamelsOnFieldBottom()
        {
            tester.PutCamelsOnTop(CamelsSecondPack.ToList());
            CollectionAssert.AreEqual(CamelsSecondPack, tester.FieldCamels);

            tester.PutCamelsOnBottom(CamelsFirstPack.ToList());
            CollectionAssert.AreEqual(CamelsBothPacks, tester.FieldCamels);
        }

        #endregion Field bottom
    }
}
#endif