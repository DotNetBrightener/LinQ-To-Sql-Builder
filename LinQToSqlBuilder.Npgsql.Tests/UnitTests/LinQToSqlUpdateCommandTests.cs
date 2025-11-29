using DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.Entities;
using Shouldly;
using Xunit;

namespace DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.UnitTests;

public class LinQToSqlUpdateCommandTests
{
    static LinQToSqlUpdateCommandTests()
    {
        NpgsqlSqlBuilder.Initialize();
    }

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

        query.CommandText.ShouldBe("UPDATE \"public\".\"Users\" " +
                                   "SET " +
                                   "\"Email\" = REPLACE(\"Email\", @Param1, @Param2), " +
                                   "\"LastChangePassword\" = @Param3, " +
                                   "\"FailedLogIns\" = \"FailedLogIns\" - @Param4 " +
                                   "WHERE \"public\".\"Users\".\"Id\" = @Param5");
    }
}