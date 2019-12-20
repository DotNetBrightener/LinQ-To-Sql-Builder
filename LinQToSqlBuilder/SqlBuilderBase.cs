/* License: http://www.apache.org/licenses/LICENSE-2.0 */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DotNetBrightener.LinQToSqlBuilder.Adapter;
using DotNetBrightener.LinQToSqlBuilder.Builder;
using DotNetBrightener.LinQToSqlBuilder.Resolver;
using DotNetBrightener.LinQToSqlBuilder.ValueObjects;

namespace DotNetBrightener.LinQToSqlBuilder
{
    /// <summary>
    /// Represents the basic operations / properties to generate the SQL queries
    /// </summary>
    public abstract class SqlBuilderBase
    {
        internal static ISqlAdapter DefaultAdapter = new SqlServerAdapter();
        internal SqlQueryBuilder Builder;
        internal LambdaResolver Resolver;

        internal SqlOperations Operation
        {
            get => Builder.Operation;
            set => Builder.Operation = value;
        }

        internal SqlQueryBuilder SqlBuilder => Builder;

        public string CommandText => Regex.Replace(Builder.CommandText, "\\s+", " ");

        public IDictionary<string, object> CommandParameters => Builder.Parameters;

        public string[] SplitColumns => Builder.SplitColumns.ToArray();

        public static void SetAdapter(SqlAdapter adapter)
        {
            DefaultAdapter = GetAdapterInstance(adapter);
        }

        private static ISqlAdapter GetAdapterInstance(SqlAdapter adapter)
        {
            switch (adapter)
            {
                case SqlAdapter.SqlServer:
                    return new SqlServerAdapter();
                default:
                    throw new ArgumentException("The specified Sql Adapter was not recognized");
            }
        }
    }
}
