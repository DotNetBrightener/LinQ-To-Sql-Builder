namespace DotNetBrightener.LinQToSqlBuilder.Adapter;

/// <summary>
///     SQL adapter provides database-specific functionality related to SQL syntax.
///     Implement this interface to add support for different database providers.
/// </summary>
public interface ISqlAdapter
{
    /// <summary>
    ///     Gets the default schema name for the database (e.g., "dbo" for SQL Server, "public" for PostgreSQL).
    ///     Return null or empty string if no default schema should be used.
    /// </summary>
    string DefaultSchema { get; }

    /// <summary>
    ///     Generates a SELECT query string.
    /// </summary>
    /// <param name="selection">The columns to select.</param>
    /// <param name="source">The table source with any JOIN expressions.</param>
    /// <param name="conditions">The WHERE clause conditions.</param>
    /// <param name="order">The ORDER BY clause.</param>
    /// <param name="grouping">The GROUP BY clause.</param>
    /// <param name="having">The HAVING clause.</param>
    /// <returns>The complete SELECT query string.</returns>
    string QueryString(string selection, string source,   string conditions,
                       string order,     string grouping, string having);

    /// <summary>
    ///     Generates a paginated SELECT query string.
    /// </summary>
    /// <param name="selection">The columns to select.</param>
    /// <param name="source">The table source with any JOIN expressions.</param>
    /// <param name="conditions">The WHERE clause conditions.</param>
    /// <param name="order">The ORDER BY clause.</param>
    /// <param name="pageSize">The number of rows per page.</param>
    /// <param name="pageIndex">The page index (0-based).</param>
    /// <returns>The complete paginated SELECT query string.</returns>
    string QueryStringPage(string selection, string source, string conditions, string order,
                           int    pageSize,  int    pageIndex = 0);

    /// <summary>
    ///     Generates an INSERT command.
    /// </summary>
    /// <param name="target">The target table name.</param>
    /// <param name="values">The list of column-value dictionaries for insertion.</param>
    /// <param name="outputIdentityColumn">The identity column to output after insert (optional).</param>
    /// <returns>The complete INSERT command string.</returns>
    string InsertCommand(string target, List<Dictionary<string, object>> values, string outputIdentityColumn = "");

    /// <summary>
    ///     Generates an INSERT FROM SELECT command.
    /// </summary>
    /// <param name="target">The target table name.</param>
    /// <param name="source">The source table name.</param>
    /// <param name="values">The column mappings from source to target.</param>
    /// <param name="conditions">The WHERE clause conditions for the source query.</param>
    /// <returns>The complete INSERT FROM SELECT command string.</returns>
    string InsertFromCommand(string target, string source, List<Dictionary<string, object>> values,
                             string conditions);

    /// <summary>
    ///     Generates an UPDATE command.
    /// </summary>
    /// <param name="updates">The SET clause with column assignments.</param>
    /// <param name="source">The table source.</param>
    /// <param name="conditions">The WHERE clause conditions.</param>
    /// <returns>The complete UPDATE command string.</returns>
    string UpdateCommand(string updates, string source, string conditions);

    /// <summary>
    ///     Generates a DELETE command.
    /// </summary>
    /// <param name="source">The table source.</param>
    /// <param name="conditions">The WHERE clause conditions.</param>
    /// <returns>The complete DELETE command string.</returns>
    string DeleteCommand(string source, string conditions);

    /// <summary>
    ///     Formats a table name with appropriate quoting/escaping for the database.
    /// </summary>
    /// <param name="tableName">The raw table name.</param>
    /// <returns>The properly quoted table name.</returns>
    string Table(string tableName);

    /// <summary>
    ///     Formats a field/column name with appropriate quoting/escaping for the database.
    /// </summary>
    /// <param name="fieldName">The raw field name.</param>
    /// <returns>The properly quoted field name.</returns>
    string Field(string fieldName);

    /// <summary>
    ///     Formats a fully qualified field name (table.column) with appropriate quoting/escaping.
    /// </summary>
    /// <param name="tableName">The table name.</param>
    /// <param name="fieldName">The field name.</param>
    /// <returns>The properly quoted table.field reference.</returns>
    string Field(string tableName, string fieldName);

    /// <summary>
    ///     Formats a parameter placeholder for the database (e.g., @param for SQL Server, $1 for PostgreSQL).
    /// </summary>
    /// <param name="parameterId">The parameter identifier.</param>
    /// <returns>The formatted parameter placeholder.</returns>
    string Parameter(string parameterId);

    /// <summary>
    ///     Generates the output clause for returning identity values after INSERT.
    ///     Return empty string if the database doesn't support this feature.
    /// </summary>
    /// <param name="fieldName">The identity field name.</param>
    /// <returns>The output clause string, or empty if not supported.</returns>
    string OutputInsertedIdentity(string fieldName);

    /// <summary>
    ///     Generates a REPLACE function call for string replacement in UPDATE statements.
    /// </summary>
    /// <param name="fieldName">The field to update.</param>
    /// <param name="findWhatParam">The parameter for the search string.</param>
    /// <param name="replaceWithParam">The parameter for the replacement string.</param>
    /// <returns>The REPLACE function expression.</returns>
    string ReplaceFunction(string fieldName, string findWhatParam, string replaceWithParam);
}

/// <summary>
///     Enumeration of SQL operation types.
/// </summary>
public enum SqlOperations
{
    /// <summary>
    ///     SELECT query operation.
    /// </summary>
    Query,

    /// <summary>
    ///     INSERT operation.
    /// </summary>
    Insert,

    /// <summary>
    ///     INSERT FROM SELECT operation.
    /// </summary>
    InsertFrom,

    /// <summary>
    ///     UPDATE operation.
    /// </summary>
    Update,

    /// <summary>
    ///     DELETE operation.
    /// </summary>
    Delete
}