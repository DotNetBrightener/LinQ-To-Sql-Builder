using DotNetBrightener.LinQToSqlBuilder.Npgsql.Adapter;

namespace DotNetBrightener.LinQToSqlBuilder.Npgsql;

/// <summary>
///     Provides initialization and configuration for the PostgreSQL (Npgsql) provider.
/// </summary>
public static class NpgsqlSqlBuilder
{
    private static bool _initialized;
    private static readonly object InitLock = new();

    /// <summary>
    ///     Initializes the PostgreSQL adapter as the default adapter for LinQToSqlBuilder.
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

            SqlBuilderBase.SetAdapter(NpgsqlAdapter.Instance);
            _initialized = true;
        }
    }

    /// <summary>
    ///     Gets the PostgreSQL adapter instance.
    /// </summary>
    public static NpgsqlAdapter Adapter => NpgsqlAdapter.Instance;
}

