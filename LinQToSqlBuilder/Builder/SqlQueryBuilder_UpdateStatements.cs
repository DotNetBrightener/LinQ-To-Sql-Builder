namespace DotNetBrightener.LinQToSqlBuilder.Builder
{
    /// <summary>
    /// Implements the expression building for the UPDATE statement
    /// </summary>
    internal partial class SqlQueryBuilder
    {
        /// <summary>
        /// Updates specified <see cref="fieldName"/> with assigning <see cref="value"/>
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public void UpdateAssignField(string fieldName, object value)
        {
            var paramId = NextParamId();
            AddParameter(paramId, value);
            var updateValue = $"{Adapter.Field(fieldName)} = {Adapter.Parameter(paramId)}";
            _updateValues.Add(updateValue);
        }

        /// <summary>
        /// Updates specified <see cref="fieldName"/> by replacing <see cref="findWhat"/> with <see cref="replaceWith"/>
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="findWhat"></param>
        /// <param name="replaceWith"></param>
        public void UpdateFieldReplaceString(string fieldName, object findWhat, object replaceWith)
        {
            var findWhatParam = NextParamId();
            AddParameter(findWhatParam, findWhat);

            var replaceWithParam = NextParamId();
            AddParameter(replaceWithParam, replaceWith);
            var updateValue =
                $"{Adapter.Field(fieldName)} = REPLACE({Adapter.Field(fieldName)}, {Adapter.Parameter(findWhatParam)}, {Adapter.Parameter(replaceWithParam)})";
            _updateValues.Add(updateValue);
        }

        /// <summary>
        /// Updates specified <see cref="fieldName"/> by performing the <see cref="operation"/> of the <see cref="operandValue"/> to current value
        /// </summary>
        /// <param name="fieldName">The name of field to update</param>
        /// <param name="operandValue">The other operand of the operation</param>
        /// <param name="operation">The operation</param>
        public void UpdateFieldWithOperation(string fieldName, object operandValue, string operation)
        {
            var paramId = NextParamId();
            AddParameter(paramId, operandValue);
            var updateValue = $"{Adapter.Field(fieldName)} = {Adapter.Field(fieldName)} {operation} {Adapter.Parameter(paramId)}";
            _updateValues.Add(updateValue);
        }
    }
}