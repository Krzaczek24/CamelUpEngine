using CamelUpEngine.GameObjects;
using CamelUpEngine.GameTools;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine.CamelsOnField
{
    internal class SettingCamelsOnStackTest : BaseClass
    {
        private static IReadOnlyCollection<ICamel> CamelsFirstPack { get; } = CamelMoveTester.Camels.Take(3).ToList();
        private static IReadOnlyCollection<ICamel> CamelsSecondPack { get; } = CamelMoveTester.Camels.TakeLast(3).ToList();
        private static IReadOnlyCollection<ICamel> CamelBothPacks { get; } = CamelsSecondPack.Concat(CamelsFirstPack).ToList();

        #region Field top

        [Test, Sequential]
        public void TestSettingSingleCamelsOnFieldTop()
        {
            IList<ICamel> addedCamels = new List<ICamel>();
            CollectionAssert.IsEmpty(tester.FieldCamels);
            CamelMoveTester.Camels.ToList().ForEach(camel => {
                tester.PutCamelsOnTop(new[] { camel }.ToList());
                addedCamels.Insert(0, camel);
                CollectionAssert.AreEqual(addedCamels, tester.FieldCamels);
            });
        }

        [Test, Sequential]
        public void TestSettingMultipleCamelsOnFieldTop()
        {
            tester.PutCamelsOnTop(CamelMoveTester.Camels.ToList());
            CollectionAssert.AreEqual(CamelMoveTester.Camels, tester.FieldCamels);
        }

        [Test, Sequential]
        public void TestSettingPacksOfCamelsOnFieldTop()
        {
            tester.PutCamelsOnTop(CamelsFirstPack.ToList());
            CollectionAssert.AreEqual(CamelsFirstPack, tester.FieldCamels);

            tester.PutCamelsOnTop(CamelsSecondPack.ToList());
            CollectionAssert.AreEqual(CamelBothPacks, tester.FieldCamels);
        }

        #endregion Field top

        #region Field bottom

        [Test, Sequential]
        public void TestSettingSingleCamelsOnFieldBottom()
        {
            IList<ICamel> addedCamels = new List<ICamel>();
            CollectionAssert.IsEmpty(tester.FieldCamels);
            CamelMoveTester.Camels.ToList().ForEach(camel => {
                tester.PutCamelsOnBottom(new[] { camel }.ToList());
                addedCamels.Add(camel);
                CollectionAssert.AreEqual(addedCamels, tester.FieldCamels);
            });
        }

        [Test, Sequential]
        public void TestSettingMultipleCamelsOnFieldBottom()
        {
            tester.PutCamelsOnBottom(CamelMoveTester.Camels.ToList());
            CollectionAssert.AreEqual(CamelMoveTester.Camels, tester.FieldCamels);
        }

        [Test, Sequential]
        public void TestSettingPacksOfCamelsOnFieldBottom()
        {
            tester.PutCamelsOnTop(CamelsSecondPack.ToList());
            CollectionAssert.AreEqual(CamelsSecondPack, tester.FieldCamels);

            tester.PutCamelsOnBottom(CamelsFirstPack.ToList());
            CollectionAssert.AreEqual(CamelBothPacks, tester.FieldCamels);
        }

        #endregion Field bottom
    }
}
