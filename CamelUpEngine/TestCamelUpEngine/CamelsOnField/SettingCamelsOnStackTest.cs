using CamelUpEngine;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine.CamelsOnField
{
    internal class SettingCamelsOnStackTest : BaseClass
    {
        private static IReadOnlyCollection<Camel> CamelsFirstPack { get; } = Camels.Take(3).ToList();
        private static IReadOnlyCollection<Camel> CamelsSecondPack { get; } = Camels.TakeLast(3).ToList();
        private static IReadOnlyCollection<Camel> CamelBothPacks { get; } = CamelsSecondPack.Concat(CamelsFirstPack).ToList();

        #region Field top

        [Test, Sequential]
        public void TestSettingSingleCamelsOnFieldTop()
        {
            IList<Camel> addedCamels = new List<Camel>();
            CollectionAssert.IsEmpty(field.GetCamels());
            Camels.ToList().ForEach(camel => {
                field.PutCamelsOnTop(new[] { camel });
                addedCamels.Insert(0, camel);
                CollectionAssert.AreEqual(addedCamels, field.GetCamels());
            });
        }

        [Test, Sequential]
        public void TestSettingMultipleCamelsOnFieldTop()
        {
            field.PutCamelsOnTop(Camels.ToList());
            CollectionAssert.AreEqual(Camels, field.GetCamels());
        }

        [Test, Sequential]
        public void TestSettingPacksOfCamelsOnFieldTop()
        {
            field.PutCamelsOnTop(CamelsFirstPack.ToList());
            CollectionAssert.AreEqual(CamelsFirstPack, field.GetCamels());

            field.PutCamelsOnTop(CamelsSecondPack.ToList());
            CollectionAssert.AreEqual(CamelBothPacks, field.GetCamels());
        }

        #endregion Field top

        #region Field bottom

        [Test, Sequential]
        public void TestSettingSingleCamelsOnFieldBottom()
        {
            IList<Camel> addedCamels = new List<Camel>();
            CollectionAssert.IsEmpty(field.GetCamels());
            Camels.ToList().ForEach(camel => {
                field.PutCamelsOnBottom(new[] { camel });
                addedCamels.Add(camel);
                CollectionAssert.AreEqual(addedCamels, field.GetCamels());
            });
        }

        [Test, Sequential]
        public void TestSettingMultipleCamelsOnFieldBottom()
        {
            field.PutCamelsOnBottom(Camels.ToList());
            CollectionAssert.AreEqual(Camels, field.GetCamels());
        }

        [Test, Sequential]
        public void TestSettingPacksOfCamelsOnFieldBottom()
        {
            field.PutCamelsOnTop(CamelsSecondPack.ToList());
            CollectionAssert.AreEqual(CamelsSecondPack, field.GetCamels());

            field.PutCamelsOnBottom(CamelsFirstPack.ToList());
            CollectionAssert.AreEqual(CamelBothPacks, field.GetCamels());
        }

        #endregion Field bottom
    }
}
