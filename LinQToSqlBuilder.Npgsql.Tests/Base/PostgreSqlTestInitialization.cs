using Xunit;

namespace DotNetBrightener.LinQToSqlBuilder.Npgsql.Tests.Base;

/// <summary>
///     Assembly fixture that initializes the PostgreSQL adapter before any tests run.
///     This class is automatically discovered and instantiated by xUnit.
/// </summary>
public class PostgreSqlTestInitialization : IDisposable
{
    public PostgreSqlTestInitialization()
    {
        NpgsqlSqlBuilder.Initialize();
    }

    public void Dispose()
    {
        // Cleanup if needed
    }
}

/// <summary>
///     Collection definition for tests that require PostgreSQL adapter initialization.
///     Tests in this collection will share the same PostgreSqlTestInitialization instance.
/// </summary>
[CollectionDefinition("PostgreSQL")]
public class PostgreSqlTestCollection : ICollectionFixture<PostgreSqlTestInitialization>;
