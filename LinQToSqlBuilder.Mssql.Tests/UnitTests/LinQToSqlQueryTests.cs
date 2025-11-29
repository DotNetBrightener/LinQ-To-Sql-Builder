using DotNetBrightener.LinQToSqlBuilder.Mssql.Tests.Entities;
using Shouldly;
using Xunit;

namespace DotNetBrightener.LinQToSqlBuilder.Mssql.Tests.UnitTests;

public class LinQToSqlQueryTests
{
    static LinQToSqlQueryTests()
    {
        SqlServerSqlBuilder.Initialize();
    }

    [Fact]
    public void QueryCount()
    {
        var query = SqlBuilder.Count<User>(u => u.Id)
                              .Where(u => u.Id > 10);

        query.CommandText.ShouldBe($"SELECT COUNT([dbo].[Users].[Id]) FROM [dbo].[Users] " +
                                   $"WHERE [dbo].[Users].[Id] > @Param1");

        query = SqlBuilder.Count<User>()
                          .Where(u => u.Id > 10);

        query.CommandText.ShouldBe($"SELECT COUNT(*) FROM [dbo].[Users] " +
                                   $"WHERE [dbo].[Users].[Id] > @Param1");
    }

    [Fact]
    public void QueryWithPagination()
    {
        var query = SqlBuilder.Select<User>()
                              .OrderBy(u => u.Id)
                              .Take(10);

        query.CommandText.ShouldBe($"SELECT TOP(10) [dbo].[Users].* FROM [dbo].[Users] ORDER BY [dbo].[Users].[Id]");
    }

    [Fact]
    public void QueryFieldsWithPagination()
    {
        var query = SqlBuilder.Select<User, UserViewModel>(user => new UserViewModel
                               {
                                   Email     = user.Email,
                                   FirstName = user.FirstName,
                                   LastName  = user.LastName,
                                   Id        = user.Id
                               })
                              .Where(u => !u.RecordDeleted)
                              .OrderBy(u => u.Id)
                              .Take(10);

        query.CommandText.ShouldBe($"SELECT TOP(10) [dbo].[Users].[Email], " +
                                   $"[dbo].[Users].[FirstName], " +
                                   $"[dbo].[Users].[LastName], " +
                                   $"[dbo].[Users].[Id] " +
                                   $"FROM [dbo].[Users] " +
                                   $"WHERE NOT [dbo].[Users].[RecordDeleted] = @Param1 " +
                                   $"ORDER BY [dbo].[Users].[Id]");
    }

    [Fact]
    public void QueryWithPagination2()
    {
        var query = SqlBuilder.Select<User>()
                              .Where(u => u.ModifiedDate > DateTimeOffset.Now.Date.AddDays(-50))
                              .OrderBy(u => u.Id)
                              .Take(10)
                              .Skip(1);


        query.CommandText
             .ShouldBe($"SELECT [dbo].[Users].* " +
                       $"FROM [dbo].[Users] " +
                       $"WHERE [dbo].[Users].[ModifiedDate] > @Param1 " +
                       $"ORDER BY [dbo].[Users].[Id] " +
                       $"OFFSET 10 ROWS FETCH NEXT 10 ROWS ONLY");
    }

    [Fact]
    public void FindByFieldValue()
    {
        var userEmail = "user@domain1.com";

        var query = SqlBuilder.Select<User>()
                              .Where(user => user.Email == userEmail);

        query.CommandText
             .ShouldBe("SELECT [dbo].[Users].* FROM [dbo].[Users] WHERE [dbo].[Users].[Email] = @Param1");

        query.CommandParameters.First().Value.ShouldBe(userEmail);
    }

    [Fact]
    public void FindByFieldValueAndGetOnlyOneResult()
    {
        var userEmail = "user@domain1.com";

        var query = SqlBuilder.SelectSingle<User>()
                              .Where(user => user.Email == userEmail);

        query.CommandText
             .ShouldBe("SELECT TOP(1) [dbo].[Users].* FROM [dbo].[Users] WHERE [dbo].[Users].[Email] = @Param1");

        query.CommandParameters.First().Value.ShouldBe(userEmail);
    }

    [Fact]
    public void FindByFieldValueLike()
    {
        const string searchTerm = "domain.com";

        var query = SqlBuilder.Select<User>()
                              .Where(user => user.Email.Contains(searchTerm));

        query.CommandText
             .ShouldBe("SELECT [dbo].[Users].* " +
                       "FROM [dbo].[Users] " +
                       "WHERE [dbo].[Users].[Email] LIKE @Param1");
    }

    [Fact]
    public void FindByJoinedEntityValue()
    {
        var email   = $"someemail@domain.com";
        var groupId = 3;
        var query = SqlBuilder.Select<User>()
                              .Join<UserUserGroup>((@user, @group) => user.Id == group.UserId)
                              .Join<UserGroup>((group,     g) => group.UserGroupId == g.Id)
                              .Where(group => group.Id == groupId);

        query.CommandText
             .ShouldBe("SELECT [dbo].[Users].*, [dbo].[UsersUserGroup].*, [dbo].[UsersGroup].* " +
                       "FROM [dbo].[Users] " +
                       "JOIN [dbo].[UsersUserGroup] ON [dbo].[Users].[Id] = [dbo].[UsersUserGroup].[UserId] " +
                       "JOIN [dbo].[UsersGroup] ON [dbo].[UsersUserGroup].[UserGroupId] = [dbo].[UsersGroup].[Id] " +
                       "WHERE [dbo].[UsersGroup].[Id] = @Param1");


        var query2 = SqlBuilder.Select<User>()
                               .Where(user => user.Email == email)
                               .Join<UserUserGroup>((@user, @group) => user.Id == group.UserId)
                               .Join<UserGroup>((group,     g) => group.UserGroupId == g.Id)
                               .Where(group => group.Id == groupId);

        query2.CommandText
              .ShouldBe("SELECT [dbo].[Users].*, [dbo].[UsersUserGroup].*, [dbo].[UsersGroup].* " +
                        "FROM [dbo].[Users] " +
                        "JOIN [dbo].[UsersUserGroup] ON [dbo].[Users].[Id] = [dbo].[UsersUserGroup].[UserId] " +
                        "JOIN [dbo].[UsersGroup] ON [dbo].[UsersUserGroup].[UserGroupId] = [dbo].[UsersGroup].[Id] " +
                        "WHERE [dbo].[Users].[Email] = @Param1 " +
                        "AND [dbo].[UsersGroup].[Id] = @Param2");
    }

    [Fact]
    public void OrderEntitiesByField()
    {
        var query = SqlBuilder.Select<UserGroup>()
                              .OrderBy(g => g.Name);

        query.CommandText
             .ShouldBe("SELECT [dbo].[UsersGroup].* " +
                       "FROM [dbo].[UsersGroup] " +
                       "ORDER BY [dbo].[UsersGroup].[Name]");
    }

    [Fact]
    public void OrderEntitiesByFieldDescending()
    {
        var query = SqlBuilder.Select<UserGroup>()
                              .OrderByDescending(g => g.Name);


        query.CommandText
             .ShouldBe("SELECT [dbo].[UsersGroup].* " +
                       "FROM [dbo].[UsersGroup] " +
                       "ORDER BY [dbo].[UsersGroup].[Name] DESC");
    }
}