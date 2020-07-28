using System;
using DotNetBrightener.LinQToSqlBuilder;
using LinQToSqlBuilder.DataAccessLayer.Tests.Entities;
using NUnit.Framework;

namespace LinQToSqlBuilder.DataAccessLayer.Tests
{
    public class LinQToSqlUpdateCommandTests
    {
        [Test]
        public void UpdateByFieldValue()
        {
            //using var scope = new TransactionScope();
            var userEmail = "user@email.com";
            var    userId    = 5;
            var query = SqlBuilder.Update<User>(_ => new User
                                   {
                                       Email =
                                           _.Email.Replace("oldemail@domain.com", "newEmail@domain.com"),
                                       LastChangePassword = DateTimeOffset.Now.AddDays(-1),
                                       FailedLogIns       = _.FailedLogIns - 2
                                   })
                                  .Where(user => user.Id == userId);

            Assert.AreEqual("UPDATE [dbo].[Users] " +
                            "SET " +
                            "[Email] = REPLACE([Email], @Param1, @Param2), " +
                            "[LastChangePassword] = @Param3, " +
                            "[FailedLogIns] = [FailedLogIns] - @Param4 " +
                            "WHERE [dbo].[Users].[Id] = @Param5",
                            query.CommandText);
        }
    }
}