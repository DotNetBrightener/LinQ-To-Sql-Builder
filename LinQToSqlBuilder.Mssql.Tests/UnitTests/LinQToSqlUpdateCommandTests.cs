using DotNetBrightener.LinQToSqlBuilder.Mssql.Tests.Base;
using DotNetBrightener.LinQToSqlBuilder.Mssql.Tests.Entities;
using Shouldly;
using Xunit;

namespace DotNetBrightener.LinQToSqlBuilder.Mssql.Tests.UnitTests;

[Collection("SqlServer")]
public class LinQToSqlUpdateCommandTests
{
    [Fact]
    public void UpdateByFieldValue()
    {
        //using var scope = new TransactionScope();
        var userEmail = "user@email.com";
        var userId    = 5;
        var query = SqlBuilder.Update<User>(_ => new User
                               {
                                   Email =
                                       _.Email.Replace("oldemail@domain.com", "newEmail@domain.com"),
                                   LastChangePassword = DateTimeOffset.Now.AddDays(-1),
                                   FailedLogIns       = _.FailedLogIns - 2
                               })
                              .Where(user => user.Id == userId);

        query.CommandText.ShouldBe("UPDATE [dbo].[Users] " +
                                   "SET " +
                                   "[Email] = REPLACE([Email], @Param1, @Param2), " +
                                   "[LastChangePassword] = @Param3, " +
                                   "[FailedLogIns] = [FailedLogIns] - @Param4 " +
                                   "WHERE [dbo].[Users].[Id] = @Param5");
    }
}