using NUnit.Framework;

namespace LinQToSqlBuilder.TestHelpers.Base
{
    public abstract class TestBase
    {
        [SetUp]
        public virtual void Init()
        {
            
        }

        [TearDown]
        public virtual void TearDown()
        {
            
        }
    }
}
