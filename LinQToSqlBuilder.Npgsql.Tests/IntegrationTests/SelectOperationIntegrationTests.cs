using Dapper;
using DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.Entities;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.IntegrationTests;

/// <summary>
///     Integration tests for SELECT operations using actual PostgreSQL database.
/// </summary>
public class SelectOperationIntegrationTests : PostgreSqlIntegrationTestBase
{
    public SelectOperationIntegrationTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public async Task SelectAll_ShouldReturnAllRecords()
    {
        // Arrange
        await SetupDatabaseAsync();
        await SeedUserGroupsAsync(5);

        var query = SqlBuilder.Select<UserGroup>();

        // Act
        using var connection = CreateConnection();
        var results = (await connection.QueryAsync<UserGroup>(query.CommandText, query.CommandParameters)).ToList();

        // Assert
        results.Count.ShouldBe(5);

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task SelectWithWhereClause_ShouldFilterCorrectly()
    {
        // Arrange
        await SetupDatabaseAsync();
        await SeedUserGroupsAsync(10);

        // Mark some as deleted
        using (var conn = CreateConnection())
        {
            await conn.ExecuteAsync(@"UPDATE ""UsersGroup"" SET ""IsDeleted"" = TRUE WHERE ""Id"" <= 3");
        }

        var query = SqlBuilder.Select<UserGroup>()
                              .Where(g => g.IsDeleted == false);

        // Act
        using var connection = CreateConnection();
        var results = (await connection.QueryAsync<UserGroup>(query.CommandText, query.CommandParameters)).ToList();

        // Assert
        results.Count.ShouldBe(7);
        results.ShouldAllBe(g => g.IsDeleted == false);

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task SelectWithTop_ShouldLimitResults()
    {
        // Arrange
        await SetupDatabaseAsync();
        await SeedUserGroupsAsync(10);

        var query = SqlBuilder.Select<UserGroup>()
                              .OrderBy(g => g.Id)
                              .Take(5);

        // Act
        using var connection = CreateConnection();
        var results = (await connection.QueryAsync<UserGroup>(query.CommandText, query.CommandParameters)).ToList();

        // Assert
        results.Count.ShouldBe(5);
        results[0].Id.ShouldBe(1);
        results[4].Id.ShouldBe(5);

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task SelectWithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        await SetupDatabaseAsync();
        await SeedUserGroupsAsync(20);

        var query = SqlBuilder.Select<UserGroup>()
                              .OrderBy(g => g.Id)
                              .Take(5)
                              .Skip(2); // Skip 2 pages of 5 records each (offset = 10)

        // Act
        using var connection = CreateConnection();
        var results = (await connection.QueryAsync<UserGroup>(query.CommandText, query.CommandParameters)).ToList();

        // Assert
        results.Count.ShouldBe(5);
        results[0].Id.ShouldBe(11); // Should start from 11th record
        results[4].Id.ShouldBe(15);

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task SelectSingle_ShouldReturnOneRecord()
    {
        // Arrange
        await SetupDatabaseAsync();
        await SeedUserGroupsAsync(5);

        var query = SqlBuilder.SelectSingle<UserGroup>()
                              .Where(g => g.Id == 3);

        // Act
        using var connection = CreateConnection();
        var result = await connection.QuerySingleOrDefaultAsync<UserGroup>(
            query.CommandText, query.CommandParameters);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(3);

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task SelectWithContains_ShouldFilterByPartialMatch()
    {
        // Arrange
        await SetupDatabaseAsync();
        using (var conn = CreateConnection())
        {
            await conn.ExecuteAsync(@"
                INSERT INTO ""UsersGroup"" (""Name"", ""Description"", ""IsDeleted"") VALUES
                ('Admin Group', 'Administrators', FALSE),
                ('User Group', 'Regular users', FALSE),
                ('Admin Support', 'Admin helpers', FALSE)
            ");
        }

        var query = SqlBuilder.Select<UserGroup>()
                              .Where(g => g.Name.Contains("Admin"));

        // Act
        using var connection = CreateConnection();
        var results = (await connection.QueryAsync<UserGroup>(query.CommandText, query.CommandParameters)).ToList();

        // Assert
        results.Count.ShouldBe(2);
        results.ShouldAllBe(g => g.Name.Contains("Admin"));

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task SelectCount_ShouldReturnCorrectCount()
    {
        // Arrange
        await SetupDatabaseAsync();
        await SeedUserGroupsAsync(15);

        var query = SqlBuilder.Count<UserGroup>()
                              .Where(g => g.Id > 5);

        // Act
        using var connection = CreateConnection();
        var count = await connection.ExecuteScalarAsync<int>(query.CommandText, query.CommandParameters);

        // Assert
        count.ShouldBe(10);

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task SelectWithOrderByDescending_ShouldOrderCorrectly()
    {
        // Arrange
        await SetupDatabaseAsync();
        await SeedUserGroupsAsync(5);

        var query = SqlBuilder.Select<UserGroup>()
                              .OrderByDescending(g => g.Id);

        // Act
        using var connection = CreateConnection();
        var results = (await connection.QueryAsync<UserGroup>(query.CommandText, query.CommandParameters)).ToList();

        // Assert
        results.Count.ShouldBe(5);
        results[0].Id.ShouldBe(5);
        results[4].Id.ShouldBe(1);

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task SelectWithJoin_ShouldReturnJoinedData()
    {
        // Arrange
        await SetupDatabaseAsync();
        await SeedUsersAndGroupsWithRelationshipsAsync();

        var query = SqlBuilder.Select<User>()
                              .Join<UserUserGroup>((user, ug) => user.Id == ug.UserId)
                              .Join<UserGroup>((ug, g) => ug.UserGroupId == g.Id)
                              .Where(g => g.Id == 1);

        // Act
        using var connection = CreateConnection();
        var results = (await connection.QueryAsync<User>(query.CommandText, query.CommandParameters)).ToList();

        // Assert
        results.Count.ShouldBe(2); // 2 users in group 1

        // Cleanup
        await CleanupDatabaseAsync();
    }

    private async Task SeedUserGroupsAsync(int count)
    {
        var userGroupFaker = TestDataGenerators.CreateUserGroupFaker();
        var groups = userGroupFaker.Generate(count);

        using var connection = CreateConnection();
        foreach (var group in groups)
        {
            await connection.ExecuteAsync(@"
                INSERT INTO ""UsersGroup"" (""Name"", ""Description"", ""IsUndeletable"", ""IsDeleted"",
                    ""CreatedDate"", ""ModifiedDate"", ""CreatedBy"", ""ModifiedBy"")
                VALUES (@Name, @Description, @IsUndeletable, @IsDeleted,
                    @CreatedDate, @ModifiedDate, @CreatedBy, @ModifiedBy)",
                group);
        }
    }

    private async Task SeedUsersAndGroupsWithRelationshipsAsync()
    {
        using var connection = CreateConnection();

        // Seed groups
        await connection.ExecuteAsync(@"
            INSERT INTO ""UsersGroup"" (""Name"", ""Description"", ""IsDeleted"") VALUES
            ('Group 1', 'First Group', FALSE),
            ('Group 2', 'Second Group', FALSE)
        ");

        // Seed users
        await connection.ExecuteAsync(@"
            INSERT INTO ""Users"" (""FirstName"", ""LastName"", ""Email"", ""RecordDeleted"", ""FailedLogIns"") VALUES
            ('John', 'Doe', 'john@test.com', FALSE, 0),
            ('Jane', 'Doe', 'jane@test.com', FALSE, 0),
            ('Bob', 'Smith', 'bob@test.com', FALSE, 0)
        ");

        // Seed relationships
        await connection.ExecuteAsync(@"
            INSERT INTO ""UsersUserGroup"" (""UserId"", ""UserGroupId"") VALUES
            (1, 1), (2, 1), (3, 2)
        ");
    }
}

