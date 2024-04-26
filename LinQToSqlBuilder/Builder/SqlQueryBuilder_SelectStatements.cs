using DotNetBrightener.LinQToSqlBuilder.ValueObjects;

namespace DotNetBrightener.LinQToSqlBuilder.Builder;

/// <summary>
/// Implements the SQL building for JOIN, ORDER BY, SELECT, and GROUP BY
/// </summary>
internal partial class SqlQueryBuilder
{
    public void Join(string originalTableName, string joinTableName, string leftField, string rightField)
    {
        var joinString =
            $"JOIN {Adapter.Table(joinTableName)} "+
            $"ON {Adapter.Field(originalTableName, leftField)} = {Adapter.Field(joinTableName, rightField)}";

        TableNames.Add(joinTableName);
        JoinExpressions.Add(joinString);
        SplitColumns.Add(rightField);
    }

    public void OrderBy(string tableName, string fieldName, bool desc = false)
    {
        var order = Adapter.Field(tableName, fieldName);
        if (desc)
            order += " DESC";

        OrderByList.Add(order);            
    }

    public void Select(string tableName)
    {
        var selectionString = $"{Adapter.Table(tableName)}.*";
        SelectionList.Add(selectionString);
    }

    public void Select(string tableName, string fieldName)
    {
        SelectionList.Add(Adapter.Field(tableName, fieldName));
    }

    public void Select(string tableName, string fieldName, SelectFunction selectFunction)
    {
        var selectionString = $"{selectFunction}({Adapter.Field(tableName, fieldName)})";
        SelectionList.Add(selectionString);
    }

    public void Select(SelectFunction selectFunction)
    {
        var selectionString = $"{selectFunction}(*)";
        SelectionList.Add(selectionString);
    }

    public void GroupBy(string tableName, string fieldName)
    {
        GroupByList.Add(Adapter.Field(tableName, fieldName));
    }

    public void SkipPages(int skipPages)
    {
        _pageIndex = skipPages;
    }

    public void Take(int pageSize)
    {
        _pageSize = pageSize;
    }

    private string GenerateQueryCommand()
    {
        if (!_pageSize.HasValue || _pageSize == 0)
            return Adapter.QueryString(Selection, Source, Conditions, Grouping, Having, Order);

        if (_pageIndex > 0 && OrderByList.Count == 0)
            throw new Exception("Pagination requires the ORDER BY statement to be specified");

        return Adapter.QueryStringPage(Source, Selection, Conditions, Order, _pageSize.Value, _pageIndex);
    }
}