using DotNetBrightener.LinQToSqlBuilder;
using LinQToSqlBuilder.DataAccessLayer.Tests.Entities;
using NUnit.Framework;

namespace LinQToSqlBuilder.DataAccessLayer.Tests;

[TestFixture]
public class LinQToSqlDeleteCommandTests
{
    [Test]
    public void DeleteByFieldValue()
    {
        var query = SqlBuilder.Delete<CloneUserGroup>(_ => _.IsDeleted);


        Assert.That(query.CommandText,
                    Is.EqualTo(
                               "DELETE FROM [dbo].[CloneUserGroup] WHERE [dbo].[CloneUserGroup].[IsDeleted] = @Param1"));

        Assert.That(query.CommandParameters.Values.FirstOrDefault(),
                    Is.EqualTo(true));
    }
}