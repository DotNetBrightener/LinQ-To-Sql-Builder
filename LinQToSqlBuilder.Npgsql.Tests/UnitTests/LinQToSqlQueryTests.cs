using DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.Entities;
using Shouldly;
using Xunit;

namespace DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.UnitTests;

public class LinQToSqlQueryTests
{
    static LinQToSqlQueryTests()
    {
        NpgsqlSqlBuilder.Initialize();
    }

    [Fact]
    public void QueryCount()
    {
        var query = SqlBuilder.Count<User>(u => u.Id)
                              .Where(u => u.Id > 10);

        query.CommandText.ShouldBe($"SELECT COUNT(\"public\".\"Users\".\"Id\") FROM \"public\".\"Users\" " +
                                   $"WHERE \"public\".\"Users\".\"Id\" > @Param1");

        query = SqlBuilder.Count<User>()
                          .Where(u => u.Id > 10);

        query.CommandText.ShouldBe($"SELECT COUNT(*) FROM \"public\".\"Users\" " +
                                   $"WHERE \"public\".\"Users\".\"Id\" > @Param1");
    }

    [Fact]
    public void QueryWithPagination()
    {
        var query = SqlBuilder.Select<User>()
                              .OrderBy(u => u.Id)
                              .Take(10);

        query.CommandText.ShouldBe($"SELECT \"public\".\"Users\".* FROM \"public\".\"Users\" ORDER BY \"public\".\"Users\".\"Id\" LIMIT 10");
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

        query.CommandText.ShouldBe($"SELECT \"public\".\"Users\".\"Email\", " +
                                   $"\"public\".\"Users\".\"FirstName\", " +
                                   $"\"public\".\"Users\".\"LastName\", " +
                                   $"\"public\".\"Users\".\"Id\" " +
                                   $"FROM \"public\".\"Users\" " +
                                   $"WHERE NOT \"public\".\"Users\".\"RecordDeleted\" = @Param1 " +
                                   $"ORDER BY \"public\".\"Users\".\"Id\" " +
                                   $"LIMIT 10");
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
             .ShouldBe($"SELECT \"public\".\"Users\".* " +
                       $"FROM \"public\".\"Users\" " +
                       $"WHERE \"public\".\"Users\".\"ModifiedDate\" > @Param1 " +
                       $"ORDER BY \"public\".\"Users\".\"Id\" " +
                       $"LIMIT 10 OFFSET 10");
    }

    [Fact]
    public void FindByFieldValue()
    {
        var userEmail = "user@domain1.com";

        var query = SqlBuilder.Select<User>()
                              .Where(user => user.Email == userEmail);

        query.CommandText
             .ShouldBe("SELECT \"public\".\"Users\".* FROM \"public\".\"Users\" WHERE \"public\".\"Users\".\"Email\" = @Param1");

        query.CommandParameters.First().Value.ShouldBe(userEmail);
    }

    [Fact]
    public void FindByFieldValueAndGetOnlyOneResult()
    {
        var userEmail = "user@domain1.com";

        var query = SqlBuilder.SelectSingle<User>()
                              .Where(user => user.Email == userEmail);

        query.CommandText
             .ShouldBe("SELECT \"public\".\"Users\".* FROM \"public\".\"Users\" WHERE \"public\".\"Users\".\"Email\" = @Param1 LIMIT 1");

        query.CommandParameters.First().Value.ShouldBe(userEmail);
    }

    [Fact]
    public void FindByFieldValueLike()
    {
        const string searchTerm = "domain.com";

        var query = SqlBuilder.Select<User>()
                              .Where(user => user.Email.Contains(searchTerm));

        query.CommandText
             .ShouldBe("SELECT \"public\".\"Users\".* " +
                       "FROM \"public\".\"Users\" " +
                       "WHERE \"public\".\"Users\".\"Email\" LIKE @Param1");
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
             .ShouldBe("SELECT \"public\".\"Users\".*, \"public\".\"UsersUserGroup\".*, \"public\".\"UsersGroup\".* " +
                       "FROM \"public\".\"Users\" " +
                       "JOIN \"public\".\"UsersUserGroup\" ON \"public\".\"Users\".\"Id\" = \"public\".\"UsersUserGroup\".\"UserId\" " +
                       "JOIN \"public\".\"UsersGroup\" ON \"public\".\"UsersUserGroup\".\"UserGroupId\" = \"public\".\"UsersGroup\".\"Id\" " +
                       "WHERE \"public\".\"UsersGroup\".\"Id\" = @Param1");


        var query2 = SqlBuilder.Select<User>()
                               .Where(user => user.Email == email)
                               .Join<UserUserGroup>((@user, @group) => user.Id == group.UserId)
                               .Join<UserGroup>((group,     g) => group.UserGroupId == g.Id)
                               .Where(group => group.Id == groupId);

        query2.CommandText
              .ShouldBe("SELECT \"public\".\"Users\".*, \"public\".\"UsersUserGroup\".*, \"public\".\"UsersGroup\".* " +
                        "FROM \"public\".\"Users\" " +
                        "JOIN \"public\".\"UsersUserGroup\" ON \"public\".\"Users\".\"Id\" = \"public\".\"UsersUserGroup\".\"UserId\" " +
                        "JOIN \"public\".\"UsersGroup\" ON \"public\".\"UsersUserGroup\".\"UserGroupId\" = \"public\".\"UsersGroup\".\"Id\" " +
                        "WHERE \"public\".\"Users\".\"Email\" = @Param1 " +
                        "AND \"public\".\"UsersGroup\".\"Id\" = @Param2");
    }

    [Fact]
    public void OrderEntitiesByField()
    {
        var query = SqlBuilder.Select<UserGroup>()
                              .OrderBy(g => g.Name);

        query.CommandText
             .ShouldBe("SELECT \"public\".\"UsersGroup\".* " +
                       "FROM \"public\".\"UsersGroup\" " +
                       "ORDER BY \"public\".\"UsersGroup\".\"Name\"");
    }

    [Fact]
    public void OrderEntitiesByFieldDescending()
    {
        var query = SqlBuilder.Select<UserGroup>()
                              .OrderByDescending(g => g.Name);


        query.CommandText
             .ShouldBe("SELECT \"public\".\"UsersGroup\".* " +
                       "FROM \"public\".\"UsersGroup\" " +
                       "ORDER BY \"public\".\"UsersGroup\".\"Name\" DESC");
    }
}