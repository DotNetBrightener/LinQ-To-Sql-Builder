using System.Data;
using DotNetBrightener.TestHelpers.PostgreSql;
using Npgsql;
using Xunit.Abstractions;

namespace DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.IntegrationTests;

/// <summary>
///     Base class for PostgreSQL integration tests using Testcontainers.
///     Provides database connection management and schema setup/teardown.
///     Each test class gets its own container and can run in parallel with other test classes.
/// </summary>
public abstract class PostgreSqlIntegrationTestBase : PostgreSqlServerBaseXUnitTest
{
    private static bool _adapterInitialized;
    private static readonly object AdapterLock = new();

    protected ITestOutputHelper TestOutputHelper { get; }

    protected PostgreSqlIntegrationTestBase(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
        TestOutputHelper = testOutputHelper;

        // Thread-safe initialization of the PostgreSQL adapter (only needs to happen once per process)
        if (!_adapterInitialized)
        {
            lock (AdapterLock)
            {
                if (!_adapterInitialized)
                {
                    NpgsqlSqlBuilder.Initialize();
                    _adapterInitialized = true;
                }
            }
        }
    }

    /// <summary>
    ///     Creates a new database connection using the container's connection string.
    /// </summary>
    protected IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(ConnectionString);
        connection.Open();
        return connection;
    }

    /// <summary>
    ///     Ensures the database is ready by waiting for connection.
    ///     Includes retry logic to handle container startup timing.
    /// </summary>
    private async Task EnsureDatabaseReadyAsync()
    {
        const int maxRetries = 10;
        const int delayMs = 2000;

        for (var attempt = 1; attempt <= maxRetries; attempt++)
        {
            await using var connection = new NpgsqlConnection(ConnectionString);

            try
            {
                await connection.OpenAsync();
                return;
            }
            catch (NpgsqlException) when (attempt < maxRetries)
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
        await EnsureDatabaseReadyAsync();
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

