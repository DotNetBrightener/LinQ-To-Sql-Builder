using System;
using System.Collections.Generic;

namespace DotNetBrightener.LinQToSqlBuilder.Adapter
{
    /// <summary>
    /// SQL adapter provides db specific functionality related to db specific SQL syntax
    /// </summary>
    public interface ISqlAdapter
    {
        string DefaultSchema { get; }

        string QueryString(string selection, string source, string conditions, 
            string order, string grouping, string having);

        string QueryStringPage(string selection, string source, string conditions, string order,
            int pageSize, int pageIndex = 0);

        string InsertCommand(string source, List<Dictionary<string, object>> values, string output = "");

        string InsertFromCommand(string target, string source, List<Dictionary<string, object>> values,
                                 string conditions);

        string UpdateCommand(string updates, string source, string conditions);

        string DeleteCommand(string source, string conditions);

        string Table(string tableName);
        
        string Field(string fieldName);

        string Field(string tableName, string fieldName);

        string Parameter(string parameterId);

        string GetTableName<T>();

        string GetTableName(Type type);
    }

    enum SqlOperations
    {
        Query,
        Insert,
        InsertFrom,
        Update,
        Delete
    }
}
