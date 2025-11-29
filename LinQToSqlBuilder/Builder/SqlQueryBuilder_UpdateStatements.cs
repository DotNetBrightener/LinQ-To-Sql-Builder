namespace DotNetBrightener.LinQToSqlBuilder.Builder;

/// <summary>
///     Implements the expression building for the UPDATE statement.
/// </summary>
internal partial class SqlQueryBuilder
{
    /// <summary>
    ///     Updates specified field with assigning a value.
    /// </summary>
    /// <param name="fieldName">The name of the field to update.</param>
    /// <param name="value">The value to assign.</param>
    public void UpdateAssignField(string fieldName, object value)
    {
        var paramId = NextParamId();
        AddParameter(paramId, value);
        var updateValue = $"{Adapter.Field(fieldName)} = {Adapter.Parameter(paramId)}";
        _updateValues.Add(updateValue);
    }

    /// <summary>
    ///     Updates specified field by replacing a substring with another string.
    /// </summary>
    /// <param name="fieldName">The name of the field to update.</param>
    /// <param name="findWhat">The substring to find.</param>
    /// <param name="replaceWith">The string to replace with.</param>
    public void UpdateFieldReplaceString(string fieldName, object findWhat, object replaceWith)
    {
        var findWhatParam = NextParamId();
        AddParameter(findWhatParam, findWhat);

        var replaceWithParam = NextParamId();
        AddParameter(replaceWithParam, replaceWith);

        var replaceExpression = Adapter.ReplaceFunction(
            Adapter.Field(fieldName),
            Adapter.Parameter(findWhatParam),
            Adapter.Parameter(replaceWithParam));

        var updateValue = $"{Adapter.Field(fieldName)} = {replaceExpression}";
        _updateValues.Add(updateValue);
    }

    /// <summary>
    ///     Updates specified field by performing an arithmetic operation with the operand value.
    /// </summary>
    /// <param name="fieldName">The name of field to update.</param>
    /// <param name="operandValue">The other operand of the operation.</param>
    /// <param name="operation">The arithmetic operation (+, -, *, /).</param>
    public void UpdateFieldWithOperation(string fieldName, object operandValue, string operation)
    {
        var paramId = NextParamId();
        AddParameter(paramId, operandValue);
        var updateValue = $"{Adapter.Field(fieldName)} = {Adapter.Field(fieldName)} {operation} {Adapter.Parameter(paramId)}";
        _updateValues.Add(updateValue);
    }
}