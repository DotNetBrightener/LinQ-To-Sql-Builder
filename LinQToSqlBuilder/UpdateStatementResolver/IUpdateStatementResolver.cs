using System.Linq.Expressions;
using System.Reflection;
using DotNetBrightener.LinQToSqlBuilder.Builder;

namespace DotNetBrightener.LinQToSqlBuilder.UpdateStatementResolver;

internal interface IUpdateStatementResolver
{
    MethodInfo SupportedMethod { get; }

    void ResolveStatement(SqlQueryBuilder      builder,
                          MethodCallExpression callExpression,
                          object[]             arguments);
}