using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetBrightener.LinQToSqlBuilder.Builder
{
    /// <summary>
    /// Implements the expression building for the WHERE statement
    /// </summary>
    internal partial class SqlQueryBuilder
    {
        public void BeginExpression()
        {
            WhereConditions.Add("(");
        }

        public void EndExpression()
        {
            WhereConditions.Add(")");
        }

        public void And()
        {
            if (WhereConditions.Count > 0)
                WhereConditions.Add(" AND ");
        }

        public void Or()
        {
            if (WhereConditions.Count > 0)
                WhereConditions.Add(" OR ");
        }

        public void Not()
        {
            WhereConditions.Add(" NOT ");
        }

        public void QueryByField(string tableName, string fieldName, string op, object fieldValue)
        {
            var paramId = NextParamId();
            string newCondition = string.Format("{0} {1} {2}",
                Adapter.Field(tableName, fieldName),
                op,
                Adapter.Parameter(paramId));

            WhereConditions.Add(newCondition);
            AddParameter(paramId, fieldValue);
        }

        public void QueryByFieldLike(string tableName, string fieldName, string fieldValue)
        {
            var paramId = NextParamId();
            string newCondition = string.Format("{0} LIKE {1}",
                Adapter.Field(tableName, fieldName),
                Adapter.Parameter(paramId));

            WhereConditions.Add(newCondition);
            AddParameter(paramId, fieldValue);
        }

        public void QueryByFieldNull(string tableName, string fieldName)
        {
            WhereConditions.Add(string.Format("{0} IS NULL", Adapter.Field(tableName, fieldName)));
        }

        public void QueryByFieldNotNull(string tableName, string fieldName)
        {
            WhereConditions.Add(string.Format("{0} IS NOT NULL", Adapter.Field(tableName, fieldName)));
        }

        public void QueryByFieldComparison(string leftTableName, string leftFieldName, string op,
            string rightTableName, string rightFieldName)
        {
            string newCondition = string.Format("{0} {1} {2}",
            Adapter.Field(leftTableName, leftFieldName),
            op,
            Adapter.Field(rightTableName, rightFieldName));

            WhereConditions.Add(newCondition);
        }

        public void QueryByIsIn(string tableName, string fieldName, SqlBuilderBase sqlQuery)
        {
            var innerQuery = sqlQuery.CommandText;            
            foreach (var param in sqlQuery.CommandParameters)
            {
                var innerParamKey = "Inner" + param.Key;
                innerQuery = Regex.Replace(innerQuery, param.Key, innerParamKey);
                AddParameter(innerParamKey, param.Value);
            }

            var newCondition = string.Format("{0} IN ({1})", Adapter.Field(tableName, fieldName), innerQuery);

            WhereConditions.Add(newCondition);
        }

        public void QueryByIsIn(string tableName, string fieldName, IEnumerable<object> values)
        {
            var paramIds = values.Select(x =>
                                             {
                                                 var paramId = NextParamId();
                                                 AddParameter(paramId, x);
                                                 return Adapter.Parameter(paramId);
                                             });

            var newCondition = string.Format("{0} IN ({1})", Adapter.Field(tableName, fieldName), string.Join(",", paramIds));
            WhereConditions.Add(newCondition);
        }
    }
}
