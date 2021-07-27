using DotNetBrightener.LinQToSqlBuilder;
using LinQToSqlBuilder.TestHelpers.Entities;
using NUnit.Framework;

namespace LinQToSqlBuilder.DataAccessLayer.Npgsql.Tests
{
    [TestFixture]
    public class LinQToSqlDeleteCommandTests
    {
        [Test]
        public void DeleteByFieldValue()
        {
            var query = SqlBuilder.Delete<CloneUserGroup>(_ => _.IsDeleted);


            Assert.AreEqual(query.CommandText,
                            "DELETE FROM [dbo].[CloneUserGroup] WHERE [dbo].[CloneUserGroup].[IsDeleted] = @Param1");
        }
    }
}