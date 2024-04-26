using DotNetBrightener.LinQToSqlBuilder.Adapter;
using DotNetBrightener.LinQToSqlBuilder.Resolver;

namespace DotNetBrightener.LinQToSqlBuilder.Builder;

internal partial class SqlQueryBuilder
{
    public void InsertTo<TTo>()
    {
        TableNames.Add(LambdaResolver.GetTableName<TTo>());
    }

    /// <summary>
    /// Updates specified <see cref="fieldName"/> with assigning <see cref="value"/>
    /// </summary>
    /// <param name="fieldName"></param>
    /// <param name="value"></param>
    public void AssignInsertField(string fieldName, object value)
    {
        var updateValue = "";
        switch (Operation)
        {
            case SqlOperations.Insert:
            case SqlOperations.InsertFrom:
                var paramId = NextParamId();
                AddParameter(paramId, value);
                updateValue = $"{Adapter.Parameter(paramId)}";
                break;
                
            default:
                throw new NotSupportedException();
        }

        var lastInsertRecord = InsertValues.LastOrDefault() ?? NextInsertRecord();
        lastInsertRecord.Add(Adapter.Field(fieldName), updateValue);
    }

    public Dictionary<string, object> NextInsertRecord()
    {
        var nextInsertRecord = new Dictionary<string, object>();
        InsertValues.Add(nextInsertRecord);
        return nextInsertRecord;
    }

    public void AssignInsertFieldFromSource(string fieldName, string sourceTableName, string sourceFieldName, string op)
    {
        var lastInsertRecord = InsertValues.LastOrDefault() ?? NextInsertRecord();

        var updateValue = $"{Adapter.Field(sourceTableName, sourceFieldName)}";

        lastInsertRecord.Add(Adapter.Field(fieldName), updateValue);
    }

    public void OutputInsertIdentity(string fieldName)
    {
        _insertOutput = Adapter.Field(fieldName);
    }
}