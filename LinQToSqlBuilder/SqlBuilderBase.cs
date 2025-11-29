using System.Text.RegularExpressions;
using DotNetBrightener.LinQToSqlBuilder.Adapter;
using DotNetBrightener.LinQToSqlBuilder.Builder;
using DotNetBrightener.LinQToSqlBuilder.Resolver;

namespace DotNetBrightener.LinQToSqlBuilder;

/// <summary>
///     Represents the basic operations / properties to generate the SQL queries.
/// </summary>
public abstract class SqlBuilderBase
{
    private static ISqlAdapter _defaultAdapter;

    internal SqlQueryBuilder Builder;
    internal LambdaResolver  Resolver;

    /// <summary>
    ///     Gets the default SQL adapter used for query generation.
    ///     Throws an exception if no adapter has been registered.
    /// </summary>
    internal static ISqlAdapter DefaultAdapter
    {
        get
        {
            if (_defaultAdapter == null)
            {
                throw new InvalidOperationException(
                    "No SQL adapter has been configured. " +
                    "Please call SqlBuilderBase.SetAdapter() with a valid adapter instance, " +
                    "or reference a database-specific package such as LinQToSqlBuilder.Mssql.");
            }
            return _defaultAdapter;
        }
    }

    internal SqlOperations Operation
    {
        get => Builder.Operation;
        set => Builder.Operation = value;
    }

    internal SqlQueryBuilder SqlBuilder => Builder;

    /// <summary>
    ///     Gets the generated SQL command text.
    /// </summary>
    public string CommandText => Regex.Replace(Builder.CommandText, "\\s+", " ").Trim();

    /// <summary>
    ///     Gets the parameters to be used with the SQL command.
    /// </summary>
    public IDictionary<string, object> CommandParameters => Builder.Parameters;

    /// <summary>
    ///     Gets the split columns for multi-entity result mapping.
    /// </summary>
    public string[] SplitColumns => Builder.SplitColumns.ToArray();

    /// <summary>
    ///     Sets the SQL adapter to use for query generation.
    ///     This must be called before using any SqlBuilder methods.
    /// </summary>
    /// <param name="adapter">The SQL adapter instance to use.</param>
    /// <exception cref="ArgumentNullException">Thrown when adapter is null.</exception>
    public static void SetAdapter(ISqlAdapter adapter)
    {
        _defaultAdapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
    }

    /// <summary>
    ///     Checks if a SQL adapter has been configured.
    /// </summary>
    /// <returns>True if an adapter is configured, false otherwise.</returns>
    public static bool HasAdapter() => _defaultAdapter != null;

    /// <summary>
    ///     Resets the adapter to null (mainly for testing purposes).
    /// </summary>
    internal static void ResetAdapter()
    {
        _defaultAdapter = null;
    }
}