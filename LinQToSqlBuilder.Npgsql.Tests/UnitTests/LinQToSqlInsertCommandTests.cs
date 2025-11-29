using DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.Entities;
using Shouldly;
using Xunit;

namespace DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.UnitTests;

public class LinQToSqlInsertCommandTests
{
    static LinQToSqlInsertCommandTests()
    {
        NpgsqlSqlBuilder.Initialize();
    }

    [Fact]
    public void InsertSingleRecord()
    {
        var query = SqlBuilder.Insert<UserGroup>(_ => new UserGroup
        {
            CreatedBy   = "TestSystem",
            CreatedDate = DateTimeOffset.Now,
            Description = "Created from Test System",
            Name        = "TestUserGroup",
            IsDeleted   = false
        });

        query.CommandText
             .ShouldBe("INSERT INTO \"public\".\"UsersGroup\" (\"CreatedBy\", \"CreatedDate\", \"Description\", \"Name\", \"IsDeleted\") " +
                       "VALUES (@Param1, @Param2, @Param3, @Param4, @Param5)");
    }

    [Fact]
    public void InsertSingleRecordWithOutputIdentity()
    {
        var query = SqlBuilder.Insert<UserGroup>(_ => new UserGroup
                               {
                                   CreatedBy   = "TestSystem",
                                   CreatedDate = DateTimeOffset.Now,
                                   Description = "Created from Test System",
                                   Name        = "TestUserGroup",
                                   IsDeleted   = false
                               })
                              .OutputIdentity();

        query.CommandText
             .ShouldBe("INSERT INTO \"public\".\"UsersGroup\" (\"CreatedBy\", \"CreatedDate\", \"Description\", \"Name\", \"IsDeleted\") " +
                       "VALUES (@Param1, @Param2, @Param3, @Param4, @Param5) " +
                       "RETURNING \"Id\"");
    }

    [Fact]
    public void InsertMultipleRecords()
    {
        var query = SqlBuilder.InsertMany<UserGroup>(_ => new[]
        {
            new UserGroup
            {
                CreatedBy   = "TestSystem",
                CreatedDate = DateTimeOffset.Now,
                Description = "Created from Test System",
                Name        = "TestUserGroup",
                IsDeleted   = false
            },
            new UserGroup
            {
                CreatedBy   = "TestSystem",
                CreatedDate = DateTimeOffset.Now,
                Description = "Created from Test System",
                Name        = "TestUserGroup2",
                IsDeleted   = false
            },
            new UserGroup
            {
                CreatedBy   = "TestSystem",
                CreatedDate = DateTimeOffset.Now,
                Description = "Created from Test System",
                Name        = "TestUserGroup3",
                IsDeleted   = false
            }
        });


        query.CommandText
             .ShouldBe("INSERT INTO \"public\".\"UsersGroup\" (\"CreatedBy\", \"CreatedDate\", \"Description\", \"Name\", \"IsDeleted\") " +
                       "VALUES (@Param1, @Param2, @Param3, @Param4, @Param5), " +
                       "(@Param6, @Param7, @Param8, @Param9, @Param10), " +
                       "(@Param11, @Param12, @Param13, @Param14, @Param15)");
    }

    [Fact]
    public void InsertRecordFromAnotherTables()
    {
        var query = SqlBuilder.InsertFrom<UserGroup, CloneUserGroup>(userGroup => new CloneUserGroup()
                               {
                                   CreatedBy     = "Cloning System",
                                   CreatedDate   = DateTimeOffset.Now,
                                   Description   = userGroup.Description,
                                   Name          = userGroup.Name,
                                   IsDeleted     = userGroup.IsDeleted,
                                   IsUndeletable = userGroup.IsUndeletable,
                                   OriginalId    = userGroup.Id
                               })
                              .Where(group => group.IsDeleted == false);

        query.CommandText
             .ShouldBe("INSERT INTO \"public\".\"CloneUserGroup\" (\"CreatedBy\", \"CreatedDate\", \"Description\", \"Name\", \"IsDeleted\", \"IsUndeletable\", \"OriginalId\") " +
                       "SELECT " +
                       "@Param1 as \"CreatedBy\", " +
                       "@Param2 as \"CreatedDate\", " +
                       "\"public\".\"UsersGroup\".\"Description\" as \"Description\", " +
                       "\"public\".\"UsersGroup\".\"Name\" as \"Name\", " +
                       "\"public\".\"UsersGroup\".\"IsDeleted\" as \"IsDeleted\", " +
                       "\"public\".\"UsersGroup\".\"IsUndeletable\" as \"IsUndeletable\", " +
                       "\"public\".\"UsersGroup\".\"Id\" as \"OriginalId\" " +
                       "FROM \"public\".\"UsersGroup\" " +
                       "WHERE \"public\".\"UsersGroup\".\"IsDeleted\" = @Param3");
    }
}