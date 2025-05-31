namespace DotNetBrightener.LinQToSqlBuilder.Adapter;

internal class SqlServerAdapter : SqlServerAdapterBase, ISqlAdapter
{
    public string QueryStringPage(string source,   string selection, string conditions, string order,
                                  int    pageSize, int    pageIndex = 0)
    {
        if (pageIndex == 0)
            return $"SELECT TOP({pageSize}) {selection} FROM {source} {conditions} {order}";

        return
            $@"SELECT {selection} FROM {source} {conditions} {order} OFFSET {pageSize * pageIndex} ROWS FETCH NEXT {pageSize} ROWS ONLY";
    }
}