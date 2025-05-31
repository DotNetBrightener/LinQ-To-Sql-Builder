namespace DotNetBrightener.LinQToSqlBuilder.Adapter;

/// <summary>
/// Provides functionality common to all supported SQL Server versions
/// </summary>
internal class SqlServerAdapterBase : SqlAdapterBase
{
    public string Table(string tableName)
    {
        return $"[{tableName}]";
    }

    public string Field(string fieldName)
    {
        return $"[{fieldName}]";
    }

    public string Field(string tableName, string fieldName)
    {
        return $"[{tableName}].[{fieldName}]";
    }

    public string Parameter(string parameterId)
    {
        return "@" + parameterId;
    }
}