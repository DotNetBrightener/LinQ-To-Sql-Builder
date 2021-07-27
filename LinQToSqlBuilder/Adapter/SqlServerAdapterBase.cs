using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace DotNetBrightener.LinQToSqlBuilder.Adapter
{
    /// <summary>
    /// Provides functionality common to all supported SQL Server versions
    /// </summary>
    class SqlServerAdapterBase : SqlAdapterBase
    {
        protected override string OpenQuote     => "[";
        protected override string CloseQuote    => "]";
        public override    string DefaultSchema => "dbo";

        public virtual string Table(string tableName)
        {
            return $"{OpenQuote}{tableName}{OpenQuote}";
        }

        public virtual string Field(string fieldName)
        {
            return $"{OpenQuote}{fieldName}{CloseQuote}";
        }

        public virtual string Field(string tableName, string fieldName)
        {
            return $"{OpenQuote}{tableName}{CloseQuote}.{OpenQuote}{fieldName}{CloseQuote}";
        }

        public virtual string Parameter(string parameterId)
        {
            return "@" + parameterId;
        }

        public virtual string GetTableName<T>()
        {
            return GetTableName(typeof(T));
        }

        public virtual string GetTableName(Type type)
        {
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();

            if (tableAttribute != null)
                return $"{tableAttribute.Schema ?? DefaultSchema}{CloseQuote}.{OpenQuote}{tableAttribute.Name}";
            else
                return $"{type.Name}";
        }
    }
}
