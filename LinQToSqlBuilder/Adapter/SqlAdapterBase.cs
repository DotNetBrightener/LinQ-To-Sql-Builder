namespace DotNetBrightener.LinQToSqlBuilder.Adapter;

/// <summary>
///     Abstract base class that provides common SQL query generation logic.
///     Extend this class and implement the abstract members to add support for specific database providers.
/// </summary>
public abstract class SqlAdapterBase : ISqlAdapter
{
    /// <inheritdoc />
    public abstract string DefaultSchema { get; }

    /// <inheritdoc />
    public virtual string QueryString(string selection,
                                      string source,
                                      string conditions,
                                      string order    = "",
                                      string grouping = "",
                                      string having   = "")
    {
        return $"SELECT {selection} FROM {source} {conditions} {order} {grouping} {having}"
           .Trim();
    }

    /// <inheritdoc />
    public abstract string QueryStringPage(string selection, string source, string conditions, string order,
                                           int    pageSize,  int    pageIndex = 0);

    /// <inheritdoc />
    public virtual string InsertCommand(string target, List<Dictionary<string, object>> values, string outputIdentityColumn = "")
    {
        var fieldsToInsert = values.First()
                                   .Select(rowValue => rowValue.Key)
                                   .ToList();
        var valuesToInsert = new List<string>();
        foreach (var rowValue in values)
        {
            valuesToInsert.Add(string.Join(", ", rowValue.Select(_ => _.Value)));
        }

        var outputClause = !string.IsNullOrEmpty(outputIdentityColumn)
                               ? OutputInsertedIdentity(outputIdentityColumn) + " "
                               : string.Empty;

        return
            $"INSERT INTO {target} ({string.Join(", ", fieldsToInsert)}) " +
            outputClause +
            $"VALUES ({string.Join("), (", valuesToInsert)})"
               .Trim();
    }

    /// <inheritdoc />
    public virtual string InsertFromCommand(string target, string source, List<Dictionary<string, object>> values, string conditions)
    {
        var fieldsToInsert = values.First()
                                   .Select(rowValue => rowValue.Key)
                                   .ToList();

        var valuesToInsert = new List<string>();

        foreach (var rowValue in values)
        {
            valuesToInsert.Add(string.Join(", ", rowValue.Select(_ => _.Value + " as " + _.Key)));
        }

        return
            $"INSERT INTO {target} ({string.Join(", ", fieldsToInsert)}) " +
            $"SELECT {string.Join(", ", valuesToInsert)} " +
            $"FROM {source} " +
            $"{conditions}"
               .Trim();
    }

    /// <inheritdoc />
    public virtual string UpdateCommand(string updates, string source, string conditions)
    {
        return $"UPDATE {source} " +
               $"SET {updates} " +
               $"{conditions}"
                  .Trim();
    }

    /// <inheritdoc />
    public virtual string DeleteCommand(string source, string conditions)
    {
        if (string.IsNullOrEmpty(conditions))
            throw new ArgumentNullException(nameof(conditions));

        return $"DELETE FROM {source} " +
               $"{conditions}"
                  .Trim();
    }

    /// <inheritdoc />
    public abstract string Table(string tableName);

    /// <inheritdoc />
    public abstract string Field(string fieldName);

    /// <inheritdoc />
    public abstract string Field(string tableName, string fieldName);

    /// <inheritdoc />
    public abstract string Parameter(string parameterId);

    /// <inheritdoc />
    public abstract string OutputInsertedIdentity(string fieldName);

    /// <inheritdoc />
    public abstract string ReplaceFunction(string fieldName, string findWhatParam, string replaceWithParam);
}