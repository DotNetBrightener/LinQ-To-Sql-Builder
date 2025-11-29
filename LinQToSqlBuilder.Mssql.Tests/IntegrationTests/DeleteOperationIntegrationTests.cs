using Dapper;
using DotNetBrightener.LinQToSqlBuilder.Mssql.Tests.Base;
using DotNetBrightener.LinQToSqlBuilder.Mssql.Tests.Entities;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace DotNetBrightener.LinQToSqlBuilder.Mssql.Tests.IntegrationTests;

/// <summary>
///     Integration tests for DELETE operations using actual SQL Server database.
/// </summary>
[Collection("SqlServer")]
public class DeleteOperationIntegrationTests : SqlServerIntegrationTestBase
{
    public DeleteOperationIntegrationTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public async Task DeleteSingleRecord_ShouldRemoveCorrectly()
    {
        // Arrange
        await SetupDatabaseAsync();
        var userGroupFaker = TestDataGenerators.CreateUserGroupFaker();
        var testGroup = userGroupFaker.Generate();

        using (var conn = CreateConnection())
        {
            await conn.ExecuteAsync(@"
                INSERT INTO [dbo].[UsersGroup] ([Name], [Description], [IsDeleted])
                VALUES (@Name, @Description, @IsDeleted)", testGroup);
        }

        var query = SqlBuilder.Delete<UserGroup>(g => g.Name == testGroup.Name);

        // Act
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(query.CommandText, query.CommandParameters);

        // Assert
        rowsAffected.ShouldBe(1);

        var deletedGroup = await connection.QuerySingleOrDefaultAsync<UserGroup>(
            "SELECT * FROM [dbo].[UsersGroup] WHERE [Name] = @Name",
            new { testGroup.Name });

        deletedGroup.ShouldBeNull();

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task DeleteMultipleRecords_ShouldRemoveAllMatchingRecords()
    {
        // Arrange
        await SetupDatabaseAsync();
        using (var conn = CreateConnection())
        {
            await conn.ExecuteAsync(@"
                INSERT INTO [dbo].[UsersGroup] ([Name], [Description], [IsDeleted]) VALUES 
                ('To Delete 1', 'Will be deleted', 1),
                ('To Delete 2', 'Will be deleted', 1),
                ('To Keep', 'Will not be deleted', 0)
            ");
        }

        var query = SqlBuilder.Delete<UserGroup>(g => g.IsDeleted == true);

        // Act
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(query.CommandText, query.CommandParameters);

        // Assert
        rowsAffected.ShouldBe(2);

        var remainingGroups = (await connection.QueryAsync<UserGroup>(
            "SELECT * FROM [dbo].[UsersGroup]")).ToList();

        remainingGroups.Count.ShouldBe(1);
        remainingGroups[0].Name.ShouldBe("To Keep");

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task DeleteWithComplexCondition_ShouldRemoveCorrectly()
    {
        // Arrange
        await SetupDatabaseAsync();
        using (var conn = CreateConnection())
        {
            await conn.ExecuteAsync(@"
                INSERT INTO [dbo].[UsersGroup] ([Name], [Description], [IsDeleted], [CreatedBy]) VALUES 
                ('Admin Group', 'Admin', 0, 'system'),
                ('Old Group', 'Outdated', 1, 'system'),
                ('Test Group', 'Testing', 1, 'test'),
                ('User Group', 'Users', 0, 'admin')
            ");
        }

        var query = SqlBuilder.Delete<UserGroup>(g => g.IsDeleted == true && g.CreatedBy == "system");

        // Act
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(query.CommandText, query.CommandParameters);

        // Assert
        rowsAffected.ShouldBe(1);

        var remainingGroups = (await connection.QueryAsync<UserGroup>(
            "SELECT * FROM [dbo].[UsersGroup]")).ToList();

        remainingGroups.Count.ShouldBe(3);
        remainingGroups.ShouldNotContain(g => g.Name == "Old Group");

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task DeleteUser_ShouldRemoveCorrectly()
    {
        // Arrange
        await SetupDatabaseAsync();
        var userFaker = TestDataGenerators.CreateUserFaker();
        var testUser = userFaker.Generate();

        using (var conn = CreateConnection())
        {
            await conn.ExecuteAsync(@"
                INSERT INTO [dbo].[Users] ([FirstName], [LastName], [Email], [RecordDeleted], [FailedLogIns])
                VALUES (@FirstName, @LastName, @Email, @RecordDeleted, @FailedLogIns)", testUser);
        }

        var query = SqlBuilder.Delete<User>(u => u.Email == testUser.Email);

        // Act
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(query.CommandText, query.CommandParameters);

        // Assert
        rowsAffected.ShouldBe(1);

        var deletedUser = await connection.QuerySingleOrDefaultAsync<User>(
            "SELECT * FROM [dbo].[Users] WHERE [Email] = @Email",
            new { testUser.Email });

        deletedUser.ShouldBeNull();

        // Cleanup
        await CleanupDatabaseAsync();
    }

    [Fact]
    public async Task DeleteWithNoMatchingRecords_ShouldReturnZeroAffected()
    {
        // Arrange
        await SetupDatabaseAsync();
        using (var conn = CreateConnection())
        {
            await conn.ExecuteAsync(@"
                INSERT INTO [dbo].[UsersGroup] ([Name], [Description], [IsDeleted]) VALUES 
                ('Active Group', 'Active', 0)
            ");
        }

        var query = SqlBuilder.Delete<UserGroup>(g => g.Name == "NonExistentGroup");

        // Act
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(query.CommandText, query.CommandParameters);

        // Assert
        rowsAffected.ShouldBe(0);

        var remainingGroups = (await connection.QueryAsync<UserGroup>(
            "SELECT * FROM [dbo].[UsersGroup]")).ToList();

        remainingGroups.Count.ShouldBe(1);

        // Cleanup
        await CleanupDatabaseAsync();
    }
}

