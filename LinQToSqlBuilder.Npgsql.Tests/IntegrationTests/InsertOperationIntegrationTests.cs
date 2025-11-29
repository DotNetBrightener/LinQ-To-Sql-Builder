using Dapper;
using DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.Entities;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.IntegrationTests;

/// <summary>
///     Integration tests for INSERT operations using actual PostgreSQL database.
/// </summary>
public class InsertOperationIntegrationTests : PostgreSqlIntegrationTestBase
{
    public InsertOperationIntegrationTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public async Task InsertSingleRecord_ShouldInsertDataCorrectly()
    {
        // Arrange
        await SetupDatabaseAsync();
        const string testName = "TestUserGroup";
        const string testDescription = "Test Description";
        const string testCreatedBy = "TestSystem";
        // PostgreSQL requires UTC offset for timestamp with time zone columns
        var testCreatedDate = DateTimeOffset.UtcNow;

        var query = SqlBuilder.Insert<UserGroup>(_ => new UserGroup
        {
            Name        = testName,
            Description = testDescription,
            CreatedBy   = testCreatedBy,
            CreatedDate = testCreatedDate,
            IsDeleted   = false
        });

        // Act
        using var connection = CreateConnection();
        await connection.ExecuteAsync(query.CommandText, query.CommandParameters);

        // Assert
        var insertedGroup = await connection.QuerySingleOrDefaultAsync<UserGroup>(
            @"SELECT * FROM ""UsersGroup"" WHERE ""Name"" = @Name",
            new { Name = testName });

        insertedGroup.ShouldNotBeNull();
        insertedGroup.Name.ShouldBe(testName);
        insertedGroup.Description.ShouldBe(testDescription);
        insertedGroup.CreatedBy.ShouldBe(testCreatedBy);
        insertedGroup.IsDeleted.ShouldBeFalse();

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task InsertSingleRecord_WithOutputIdentity_ShouldReturnGeneratedId()
    {
        // Arrange
        await SetupDatabaseAsync();
        const string testName = "OutputIdentityTestGroup";
        // PostgreSQL requires UTC offset for timestamp with time zone columns
        var testCreatedDate = DateTimeOffset.UtcNow;

        var query = SqlBuilder.Insert<UserGroup>(_ => new UserGroup
                               {
                                   Name        = testName,
                                   Description = "Test Description for Output Identity",
                                   CreatedBy   = "TestSystem",
                                   CreatedDate = testCreatedDate,
                                   IsDeleted   = false
                               })
                              .OutputIdentity();

        // Act
        using var connection = CreateConnection();
        var insertedId = await connection.ExecuteScalarAsync<long>(query.CommandText, query.CommandParameters);

        // Assert
        insertedId.ShouldBeGreaterThan(0);

        var insertedGroup = await connection.QuerySingleOrDefaultAsync<UserGroup>(
            @"SELECT * FROM ""UsersGroup"" WHERE ""Id"" = @Id",
            new { Id = insertedId });

        insertedGroup.ShouldNotBeNull();
        insertedGroup.Id.ShouldBe(insertedId);
        insertedGroup.Name.ShouldBe(testName);

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task InsertMultipleRecords_ShouldInsertAllRecordsCorrectly()
    {
        // Arrange
        await SetupDatabaseAsync();
        // PostgreSQL requires UTC offset for timestamp with time zone columns
        var testCreatedDate = DateTimeOffset.UtcNow;

        var query = SqlBuilder.InsertMany<UserGroup>(_ => new[]
        {
            new UserGroup
            {
                Name        = "MultiInsertGroup1",
                Description = "Description 1",
                CreatedBy   = "TestSystem",
                CreatedDate = testCreatedDate,
                IsDeleted   = false
            },
            new UserGroup
            {
                Name        = "MultiInsertGroup2",
                Description = "Description 2",
                CreatedBy   = "TestSystem",
                CreatedDate = testCreatedDate,
                IsDeleted   = false
            },
            new UserGroup
            {
                Name        = "MultiInsertGroup3",
                Description = "Description 3",
                CreatedBy   = "TestSystem",
                CreatedDate = testCreatedDate,
                IsDeleted   = false
            }
        });

        // Act
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(query.CommandText, query.CommandParameters);

        // Assert
        rowsAffected.ShouldBe(3);

        var insertedGroups = (await connection.QueryAsync<UserGroup>(
            @"SELECT * FROM ""UsersGroup""")).ToList();

        insertedGroups.Count.ShouldBe(3);
        insertedGroups.ShouldContain(g => g.Name == "MultiInsertGroup1");
        insertedGroups.ShouldContain(g => g.Name == "MultiInsertGroup2");
        insertedGroups.ShouldContain(g => g.Name == "MultiInsertGroup3");

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task InsertUser_ShouldInsertDataWithAllFields()
    {
        // Arrange
        await SetupDatabaseAsync();
        const string testEmail = "test@example.com";
        const string testFirstName = "John";
        const string testLastName = "Doe";
        // PostgreSQL requires UTC offset for timestamp with time zone columns
        var testModifiedDate = DateTimeOffset.UtcNow;

        var query = SqlBuilder.Insert<User>(_ => new User
        {
            FirstName          = testFirstName,
            LastName           = testLastName,
            Email              = testEmail,
            RecordDeleted      = false,
            LastChangePassword = testModifiedDate,
            ModifiedDate       = testModifiedDate,
            FailedLogIns       = 0
        });

        // Act
        using var connection = CreateConnection();
        await connection.ExecuteAsync(query.CommandText, query.CommandParameters);

        // Assert
        var insertedUser = await connection.QuerySingleOrDefaultAsync<User>(
            @"SELECT * FROM ""Users"" WHERE ""Email"" = @Email",
            new { Email = testEmail });

        insertedUser.ShouldNotBeNull();
        insertedUser.FirstName.ShouldBe(testFirstName);
        insertedUser.LastName.ShouldBe(testLastName);
        insertedUser.Email.ShouldBe(testEmail);
        insertedUser.FailedLogIns.ShouldBe(0);

        // Cleanup
        await CleanupDatabaseAsync();
    }
}

