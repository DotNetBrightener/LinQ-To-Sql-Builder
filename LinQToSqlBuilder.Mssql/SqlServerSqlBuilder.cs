namespace DotNetBrightener.LinQToSqlBuilder.Mssql;

/// <summary>
///     Provides initialization and configuration for the SQL Server provider.
/// </summary>
public static class SqlServerSqlBuilder
{
    private static bool _initialized;
    private static readonly object InitLock = new();

    /// <summary>
    ///     Initializes the SQL Server adapter as the default adapter for LinQToSqlBuilder.
    ///     This method should be called once at application startup before using any SqlBuilder methods.
    /// </summary>
    /// <remarks>
    ///     This method is thread-safe and can be called multiple times safely.
    ///     Subsequent calls after the first initialization will be ignored.
    /// </remarks>
    public static void Initialize()
    {
        if (_initialized)
            return;

        lock (InitLock)
        {
            if (_initialized)
                return;

            SqlBuilderBase.SetAdapter(SqlServerAdapter.Instance);
            _initialized = true;
        }
    }

    /// <summary>
    ///     Gets the SQL Server adapter instance.
    /// </summary>
    public static SqlServerAdapter Adapter => SqlServerAdapter.Instance;
}

