﻿using DotNetBrightener.LinQToSqlBuilder.Adapter;
using DotNetBrightener.LinQToSqlBuilder.Builder;
using DotNetBrightener.LinQToSqlBuilder.Resolver;
using DotNetBrightener.LinQToSqlBuilder.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        public string CommandText => Regex.Replace(Builder.CommandText, "\\s+", " ").Trim();

        public IDictionary<string, object> CommandParameters => Builder.Parameters;

        public string[] SplitColumns => Builder.SplitColumns.ToArray();

        public static void SetAdapter(DatabaseProvider provider)
        {
            DefaultAdapter = GetAdapterInstance(provider);
        }

        protected static ISqlAdapter GetAdapterInstance(DatabaseProvider provider)
        {
            switch (provider)
            {
                case DatabaseProvider.SqlServer:
                    return new SqlServerAdapter();
                case DatabaseProvider.PostgreSql:
                    return new PostgreSqlServerAdapter();
                default:
                    throw new ArgumentException("The specified Sql Adapter was not recognized");
            }
        }
    }
}
