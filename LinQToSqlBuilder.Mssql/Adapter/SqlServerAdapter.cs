using DotNetBrightener.LinQToSqlBuilder.Adapter;

namespace DotNetBrightener.LinQToSqlBuilder.Mssql;

/// <summary>
///     SQL Server adapter providing T-SQL specific syntax for query generation.
/// </summary>
public class SqlServerAdapter : SqlAdapterBase
{
    /// <summary>
    ///     Gets the singleton instance of the SQL Server adapter.
    /// </summary>
    public static SqlServerAdapter Instance { get; } = new();

    /// <inheritdoc />
    public override string DefaultSchema => "dbo";

    /// <inheritdoc />
    public override string QueryStringPage(string selection,
                                           string source,
                                           string conditions,
                                           string order,
                                           int    pageSize,
                                           int    pageIndex = 0)
    {
        if (pageIndex == 0)
            return $"SELECT TOP({pageSize}) {selection} " +
                   $"FROM {source} " +
                   $"{conditions} " +
                   $"{order}";

        return
            $"SELECT {selection} " +
            $"FROM {source} " +
            $"{conditions} " +
            $"{order} " +
            $"OFFSET {pageSize * pageIndex} ROWS FETCH NEXT {pageSize} ROWS ONLY";
    }

    /// <inheritdoc />
    public override string Table(string tableName)
    {
        return $"[{tableName}]";
    }

    /// <inheritdoc />
    public override string Field(string fieldName)
    {
        return $"[{fieldName}]";
    }

    /// <inheritdoc />
    public override string Field(string tableName, string fieldName)
    {
        return $"[{tableName}].[{fieldName}]";
    }

    /// <inheritdoc />
    public override string Parameter(string parameterId)
    {
        return "@" + parameterId;
    }

    /// <inheritdoc />
    public override string OutputInsertedIdentity(string fieldName)
    {
        return $"OUTPUT Inserted.{fieldName}";
    }

    /// <inheritdoc />
    public override string ReplaceFunction(string fieldName, string findWhatParam, string replaceWithParam)
    {
        return $"REPLACE({fieldName}, {findWhatParam}, {replaceWithParam})";
    }
}