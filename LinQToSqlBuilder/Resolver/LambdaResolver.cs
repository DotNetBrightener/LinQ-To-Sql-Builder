using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using DotNetBrightener.LinQToSqlBuilder.Adapter;
using DotNetBrightener.LinQToSqlBuilder.Builder;
using DotNetBrightener.LinQToSqlBuilder.UpdateStatementResolver;

namespace DotNetBrightener.LinQToSqlBuilder.Resolver;

/// <summary>
///     Provides methods to perform resolution to SQL expressions from given lambda expressions.
/// </summary>
internal partial class LambdaResolver
{
    private readonly Dictionary<ExpressionType, string> _operationDictionary =
        new()
        {
            {ExpressionType.Equal, "="},
            {ExpressionType.NotEqual, "!="},
            {ExpressionType.GreaterThan, ">"},
            {ExpressionType.LessThan, "<"},
            {ExpressionType.GreaterThanOrEqual, ">="},
            {ExpressionType.LessThanOrEqual, "<="}
        };

    private SqlQueryBuilder Builder { get; set; }

    public LambdaResolver(SqlQueryBuilder builder)
    {
        Builder = builder;
    }

    private static readonly List<IUpdateStatementResolver> StatementResolvers = [new StringReplaceUpdateResolver()];

    /// <summary>
    ///     Registers a custom update statement resolver.
    /// </summary>
    /// <param name="resolver">The resolver to register.</param>
    public static void RegisterResolver(IUpdateStatementResolver resolver)
    {
        StatementResolvers.Add(resolver);
    }

    /// <summary>
    ///     Gets the column name from a property selector expression.
    /// </summary>
    public static string GetColumnName<T>(Expression<Func<T, object>> selector)
    {
        return GetColumnName(GetMemberExpression(selector.Body));
    }

    /// <summary>
    ///     Gets the column name from an expression.
    /// </summary>
    public static string GetColumnName(Expression expression)
    {
        var member = GetMemberExpression(expression);
        return GetColumnName(member);
    }

    /// <summary>
    ///     Gets the column name from a member assignment expression.
    /// </summary>
    public static string GetColumnName(MemberAssignment expression)
    {
        return GetColumnName(expression.Member);
    }

    private static string GetColumnName(MemberExpression member)
    {
        var memberInfo = member.Member;
        return GetColumnName(memberInfo);
    }

    private static string GetColumnName(MemberInfo memberInfo)
    {
        var column = memberInfo.GetCustomAttribute<ColumnAttribute>();

        if (column != null)
            return $"{column.Name}";
        else
            return $"{memberInfo.Name}";
    }

    /// <summary>
    ///     Gets the table name from a type, using the adapter's default schema if no schema is specified.
    /// </summary>
    public static string GetTableName<T>()
    {
        return GetTableName(typeof(T), SqlBuilderBase.DefaultAdapter);
    }

    /// <summary>
    ///     Gets the table name from a type, using the adapter's default schema if no schema is specified.
    /// </summary>
    public static string GetTableName(Type type)
    {
        return GetTableName(type, SqlBuilderBase.DefaultAdapter);
    }

    /// <summary>
    ///     Gets the table name from a type using the provided adapter for schema resolution.
    /// </summary>
    internal static string GetTableName(Type type, ISqlAdapter adapter)
    {
        var tableAttribute = type.GetCustomAttribute<TableAttribute>();
        if (tableAttribute != null)
        {
            var schema = tableAttribute.Schema;
            if (string.IsNullOrEmpty(schema))
            {
                schema = adapter?.DefaultSchema;
            }

            if (!string.IsNullOrEmpty(schema))
            {
                return $"{schema}].[{tableAttribute.Name}";
            }

            return tableAttribute.Name;
        }
        else
        {
            return $"{type.Name}";
        }
    }

    private static string GetTableName(MemberExpression expression)
    {
        return GetTableName(expression.Expression.Type);
    }

    private static BinaryExpression GetBinaryExpression(Expression expression)
    {
        if (expression is BinaryExpression binaryExpression)
            return binaryExpression;

        throw new ArgumentException("Binary expression expected");
    }

    private static MemberExpression GetMemberExpression(Expression expression)
    {
        switch (expression.NodeType)
        {
            case ExpressionType.MemberAccess:
                return expression as MemberExpression;
            case ExpressionType.Convert:
                return GetMemberExpression((expression as UnaryExpression)?.Operand);
        }

        throw new ArgumentException("Member expression expected");
    }
}