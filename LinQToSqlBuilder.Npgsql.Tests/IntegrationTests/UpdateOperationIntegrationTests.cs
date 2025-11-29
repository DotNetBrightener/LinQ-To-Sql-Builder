using Dapper;
using DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.Entities;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.IntegrationTests;

/// <summary>
///     Integration tests for UPDATE operations using actual PostgreSQL database.
/// </summary>
public class UpdateOperationIntegrationTests : PostgreSqlIntegrationTestBase
{
    public UpdateOperationIntegrationTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public async Task UpdateSingleField_ShouldModifyCorrectly()
    {
        // Arrange
        await SetupDatabaseAsync();
        const string testName = "TestGroup";
        const string originalDescription = "Original Description";

        using (var conn = CreateConnection())
        {
            await conn.ExecuteAsync(@"
                INSERT INTO ""UsersGroup"" (""Name"", ""Description"", ""IsDeleted"")
                VALUES (@Name, @Description, @IsDeleted)",
                new { Name = testName, Description = originalDescription, IsDeleted = false });
        }

        // Use Replace method which is supported by the library
        var query = SqlBuilder.Update<UserGroup>(_ => new UserGroup
                               {
                                   Description = _.Description.Replace("Original", "Updated")
                               })
                              .Where(g => g.Name == testName);

        // Act
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(query.CommandText, query.CommandParameters);

        // Assert
        rowsAffected.ShouldBe(1);

        var updatedGroup = await connection.QuerySingleOrDefaultAsync<UserGroup>(
            @"SELECT * FROM ""UsersGroup"" WHERE ""Name"" = @Name",
            new { Name = testName });

        updatedGroup.ShouldNotBeNull();
        updatedGroup.Description.ShouldBe("Updated Description");

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task UpdateMultipleFields_ShouldModifyAllCorrectly()
    {
        // Arrange
        await SetupDatabaseAsync();
        const string testName = "TestGroupMultiField";
        const string originalDescription = "Original Description";

        using (var conn = CreateConnection())
        {
            await conn.ExecuteAsync(@"
                INSERT INTO ""UsersGroup"" (""Name"", ""Description"", ""IsDeleted"", ""ModifiedBy"")
                VALUES (@Name, @Description, @IsDeleted, @ModifiedBy)",
                new { Name = testName, Description = originalDescription, IsDeleted = false, ModifiedBy = "OriginalUser" });
        }

        // Use Replace method and DateTimeOffset.UtcNow which are supported by the library
        // PostgreSQL requires UTC offset for timestamp with time zone columns
        var query = SqlBuilder.Update<UserGroup>(_ => new UserGroup
                               {
                                   Description  = _.Description.Replace("Original", "Updated"),
                                   ModifiedBy   = _.ModifiedBy.Replace("OriginalUser", "UpdateSystem"),
                                   ModifiedDate = DateTimeOffset.UtcNow
                               })
                              .Where(g => g.Name == testName);

        // Act
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(query.CommandText, query.CommandParameters);

        // Assert
        rowsAffected.ShouldBe(1);

        var updatedGroup = await connection.QuerySingleOrDefaultAsync<UserGroup>(
            @"SELECT * FROM ""UsersGroup"" WHERE ""Name"" = @Name",
            new { Name = testName });

        updatedGroup.ShouldNotBeNull();
        updatedGroup.Description.ShouldBe("Updated Description");
        updatedGroup.ModifiedBy.ShouldBe("UpdateSystem");
        updatedGroup.ModifiedDate.ShouldNotBeNull();

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task UpdateMultipleRecords_ShouldModifyAllMatchingRecords()
    {
        // Arrange
        await SetupDatabaseAsync();
        using (var conn = CreateConnection())
        {
            await conn.ExecuteAsync(@"
                INSERT INTO ""UsersGroup"" (""Name"", ""Description"", ""IsDeleted"") VALUES
                ('Group A', 'Active Group A', FALSE),
                ('Group B', 'Active Group B', FALSE),
                ('Group C', 'Inactive Group C', TRUE)
            ");
        }

        // Use Replace method which is supported by the library
        var query = SqlBuilder.Update<UserGroup>(_ => new UserGroup
                               {
                                   Description = _.Description.Replace("Active Group", "Updated Active Group")
                               })
                              .Where(g => g.IsDeleted == false);

        // Act
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(query.CommandText, query.CommandParameters);

        // Assert
        rowsAffected.ShouldBe(2);

        var updatedGroups = (await connection.QueryAsync<UserGroup>(
            @"SELECT * FROM ""UsersGroup"" WHERE ""IsDeleted"" = FALSE")).ToList();

        updatedGroups.Count.ShouldBe(2);
        updatedGroups.ShouldAllBe(g => g.Description.Contains("Updated Active Group"));

        // Verify non-matching record was not updated
        var nonUpdatedGroup = await connection.QuerySingleOrDefaultAsync<UserGroup>(
            @"SELECT * FROM ""UsersGroup"" WHERE ""IsDeleted"" = TRUE");
        nonUpdatedGroup.Description.ShouldBe("Inactive Group C");

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task UpdateUser_ShouldModifyUserCorrectly()
    {
        // Arrange
        await SetupDatabaseAsync();
        const string originalEmail = "original@test.com";
        const string testFirstName = "John";
        const string testLastName = "Doe";

        using (var conn = CreateConnection())
        {
            await conn.ExecuteAsync(@"
                INSERT INTO ""Users"" (""FirstName"", ""LastName"", ""Email"", ""RecordDeleted"", ""FailedLogIns"")
                VALUES (@FirstName, @LastName, @Email, @RecordDeleted, @FailedLogIns)",
                new { FirstName = testFirstName, LastName = testLastName, Email = originalEmail, RecordDeleted = false, FailedLogIns = 0 });
        }

        // Use Replace method and DateTimeOffset.UtcNow which are supported by the library
        // PostgreSQL requires UTC offset for timestamp with time zone columns
        var query = SqlBuilder.Update<User>(_ => new User
                               {
                                   Email        = _.Email.Replace("original", "updated"),
                                   ModifiedDate = DateTimeOffset.UtcNow
                               })
                              .Where(u => u.Email == originalEmail);

        // Act
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(query.CommandText, query.CommandParameters);

        // Assert
        rowsAffected.ShouldBe(1);

        var updatedUser = await connection.QuerySingleOrDefaultAsync<User>(
            @"SELECT * FROM ""Users"" WHERE ""Email"" = @Email",
            new { Email = "updated@test.com" });

        updatedUser.ShouldNotBeNull();
        updatedUser.Email.ShouldBe("updated@test.com");
        updatedUser.FirstName.ShouldBe(testFirstName);
        updatedUser.LastName.ShouldBe(testLastName);

        // Cleanup
        await CleanupDatabaseAsync();
    }
}

