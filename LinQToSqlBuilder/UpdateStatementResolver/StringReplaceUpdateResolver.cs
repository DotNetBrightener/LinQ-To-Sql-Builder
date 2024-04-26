using System.Linq.Expressions;
using System.Reflection;
using DotNetBrightener.LinQToSqlBuilder.Builder;
using DotNetBrightener.LinQToSqlBuilder.Resolver;

namespace DotNetBrightener.LinQToSqlBuilder.UpdateStatementResolver;

internal class StringReplaceUpdateResolver : IUpdateStatementResolver
{
    public MethodInfo SupportedMethod
    {
        get
        {
            var method = typeof(string).GetMethods()
                                       .FirstOrDefault(_ => _.Name == nameof(string.Replace) &&
                                                            _.GetParameters().Length == 2 &&
                                                            _.GetParameters()
                                                             .All(p => p.ParameterType == typeof(string)));

            return method;
        }
    }

    public void ResolveStatement(SqlQueryBuilder      builder,
                                 MethodCallExpression callExpression,
                                 object[]             arguments)
    {
        if (arguments.Length != 2)
            throw new
                ArgumentException($"REPLACE Sql query requires 2 arguments for replacing old_string with new_string");

        var columnName = LambdaResolver.GetColumnName(callExpression.Object);

        builder.UpdateFieldReplaceString(columnName, arguments[0], arguments[1]);
    }
}