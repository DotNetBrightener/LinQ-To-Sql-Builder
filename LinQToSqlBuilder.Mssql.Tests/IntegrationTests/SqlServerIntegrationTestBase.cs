using System.Data;
using Dapper;
using DotNetBrightener.TestHelpers;
using Microsoft.Data.SqlClient;
using Xunit.Abstractions;

namespace DotNetBrightener.LinQToSqlBuilder.Mssql.Tests.IntegrationTests;

/// <summary>
///     Base class for SQL Server integration tests using Testcontainers.
///     Provides database connection management and schema setup/teardown.
///     Note: SQL Server adapter initialization is handled by the [Collection("SqlServer")] fixture.
/// </summary>
public abstract class SqlServerIntegrationTestBase : MsSqlServerBaseXUnitTest
{
    private const string TestDatabaseName = "LinQToSqlBuilderTest";

    protected ITestOutputHelper TestOutputHelper { get; }

    protected SqlServerIntegrationTestBase(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
        TestOutputHelper = testOutputHelper;
    }

    /// <summary>
    ///     Gets the connection string for the master database.
    /// </summary>
    private string GetMasterConnectionString()
    {
        // The base class ConnectionString points to a test database that may not exist
        // We need to connect to master first to create the database
        return ConnectionString.Replace($"Database={TestDatabaseName}", "Database=master")
                               .Replace("Database=MsSqlServerBaseTest", "Database=master");
    }

    /// <summary>
    ///     Gets the connection string for the test database.
    /// </summary>
    private string GetTestDatabaseConnectionString()
    {
        return ConnectionString.Replace("Database=MsSqlServerBaseTest", $"Database={TestDatabaseName}");
    }

    /// <summary>
    ///     Creates a new database connection using the container's connection string.
    /// </summary>
    protected IDbConnection CreateConnection()
    {
        var connection = new SqlConnection(GetTestDatabaseConnectionString());
        connection.Open();
        return connection;
    }

    /// <summary>
    ///     Creates the test database if it doesn't exist.
    ///     Includes retry logic to handle SQL Server startup timing.
    /// </summary>
    private async Task EnsureDatabaseExistsAsync()
    {
        const int maxRetries = 10;
        const int delayMs = 2000;

        for (var attempt = 1; attempt <= maxRetries; attempt++)
        {
            await using var masterConnection = new SqlConnection(GetMasterConnectionString());

            try
            {
                await masterConnection.OpenAsync();

                var checkDbSql = $@"
                    IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{TestDatabaseName}')
                    BEGIN
                        CREATE DATABASE [{TestDatabaseName}]
                    END";

                await masterConnection.ExecuteAsync(checkDbSql);
                return;
            }
            catch (SqlException) when (attempt < maxRetries)
            {
                await Task.Delay(delayMs);
            }
        }
    }

    /// <summary>
    ///     Sets up the database schema before tests run.
    /// </summary>
    protected async Task SetupDatabaseAsync()
    {
        await EnsureDatabaseExistsAsync();
        using var connection = CreateConnection();
        await DatabaseSchemaSetup.CreateTablesAsync(connection);
    }

    /// <summary>
    ///     Cleans up the database after tests run.
    /// </summary>
    protected async Task CleanupDatabaseAsync()
    {
        using var connection = CreateConnection();
        await DatabaseSchemaSetup.TruncateTablesAsync(connection);
    }
}

