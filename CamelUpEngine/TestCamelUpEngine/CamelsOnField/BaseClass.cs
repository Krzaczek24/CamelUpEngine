using CamelUpEngine.GameTools;
using NUnit.Framework;

namespace TestCamelUpEngine.CamelsOnField
{
    internal abstract class BaseClass
    {
        protected readonly CamelMoveTester tester = new CamelMoveTester();

        [SetUp]
        public virtual void Setup()
        {
            tester.ResetField();
        }
    }
}
