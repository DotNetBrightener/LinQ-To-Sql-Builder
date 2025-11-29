using Xunit;

namespace DotNetBrightener.LinQToSqlBuilder.Mssql.Tests.Base;

/// <summary>
///     Assembly fixture that initializes the SQL Server adapter before any tests run.
///     This class is automatically discovered and instantiated by xUnit.
/// </summary>
public class SqlServerTestInitialization : IDisposable
{
    public SqlServerTestInitialization()
    {
        SqlServerSqlBuilder.Initialize();
    }

    public void Dispose()
    {
        // Cleanup if needed
    }
}

/// <summary>
///     Collection definition for tests that require SQL Server adapter initialization.
///     Tests in this collection will share the same SqlServerTestInitialization instance.
/// </summary>
[CollectionDefinition("SqlServer")]
public class SqlServerTestCollection : ICollectionFixture<SqlServerTestInitialization>;

