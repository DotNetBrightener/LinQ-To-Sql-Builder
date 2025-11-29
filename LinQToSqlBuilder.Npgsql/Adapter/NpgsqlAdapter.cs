using DotNetBrightener.LinQToSqlBuilder.Adapter;

namespace DotNetBrightener.LinQToSqlBuilder.Npgsql;

/// <summary>
///     PostgreSQL adapter providing Npgsql-specific syntax for query generation.
/// </summary>
public class NpgsqlAdapter : SqlAdapterBase
{
    /// <summary>
    ///     Gets the singleton instance of the PostgreSQL adapter.
    /// </summary>
    public static NpgsqlAdapter Instance { get; } = new();

    /// <inheritdoc />
    public override string DefaultSchema => "public";

    /// <inheritdoc />
    public override string QueryStringPage(string selection,
                                           string source,
                                           string conditions,
                                           string order,
                                           int    pageSize,
                                           int    pageIndex = 0)
    {
        if (pageIndex == 0)
            return $"SELECT {selection} " +
                   $"FROM {source} " +
                   $"{conditions} " +
                   $"{order} " +
                   $"LIMIT {pageSize}";

        return
            $"SELECT {selection} " +
            $"FROM {source} " +
            $"{conditions} " +
            $"{order} " +
            $"LIMIT {pageSize} OFFSET {pageSize * pageIndex}";
    }

    /// <inheritdoc />
    public override string InsertCommand(string target, List<Dictionary<string, object>> values, string outputIdentityColumn = "")
    {
        var fieldsToInsert = values.First()
                                   .Select(rowValue => rowValue.Key)
                                   .ToList();
        var valuesToInsert = new List<string>();
        foreach (var rowValue in values)
        {
            valuesToInsert.Add(string.Join(", ", rowValue.Select(_ => _.Value)));
        }

        var returningClause = !string.IsNullOrEmpty(outputIdentityColumn)
                                  ? " " + OutputInsertedIdentity(outputIdentityColumn)
                                  : string.Empty;

        return
            ($"INSERT INTO {target} ({string.Join(", ", fieldsToInsert)}) " +
             $"VALUES ({string.Join("), (", valuesToInsert)})" +
             returningClause).Trim();
    }

    /// <inheritdoc />
    public override string Table(string tableName)
    {
        // Handle schema.table format - the core library uses ].[  as separator
        if (tableName.Contains("].["))
        {
            var parts = tableName.Split(new[] { "].[" }, StringSplitOptions.None);
            return $"\"{parts[0]}\".\"{parts[1]}\"";
        }

        return $"\"{tableName}\"";
    }

    /// <inheritdoc />
    public override string Field(string fieldName)
    {
        return $"\"{fieldName}\"";
    }

    /// <inheritdoc />
    public override string Field(string tableName, string fieldName)
    {
        // Handle schema.table format - the core library uses ].[  as separator
        if (tableName.Contains("].["))
        {
            var parts = tableName.Split(new[] { "].[" }, StringSplitOptions.None);
            return $"\"{parts[0]}\".\"{parts[1]}\".\"{fieldName}\"";
        }

        return $"\"{tableName}\".\"{fieldName}\"";
    }

    /// <inheritdoc />
    public override string Parameter(string parameterId)
    {
        return "@" + parameterId;
    }

    /// <inheritdoc />
    public override string OutputInsertedIdentity(string fieldName)
    {
        // fieldName is already quoted by the caller (SqlQueryBuilder.OutputInsertIdentity)
        return $"RETURNING {fieldName}";
    }

    /// <inheritdoc />
    public override string ReplaceFunction(string fieldName, string findWhatParam, string replaceWithParam)
    {
        return $"REPLACE({fieldName}, {findWhatParam}, {replaceWithParam})";
    }
}

