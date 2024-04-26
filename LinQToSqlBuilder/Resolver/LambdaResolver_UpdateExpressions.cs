using System.Linq.Expressions;

namespace DotNetBrightener.LinQToSqlBuilder.Resolver;

partial class LambdaResolver
{
    public void Update<T>(Expression<Func<T, object>> expression)
    {
        Update<T>(expression.Body);
    }

    private void Update<T>(Expression expression)
    {
        switch (expression.NodeType)
        {
            case ExpressionType.New:
                foreach (MemberExpression memberExp in (expression as NewExpression).Arguments)
                    Update<T>(memberExp);
                break;
            case ExpressionType.MemberInit:
                if (!(expression is MemberInitExpression memberInitExpression))
                    throw new ArgumentException("Invalid expression");

                foreach (var memberBinding in memberInitExpression.Bindings)
                {
                    if (memberBinding is MemberAssignment assignment)
                    {
                        Update<T>(assignment);
                    }
                }

                break;
            default:
                throw new ArgumentException("Invalid expression");
        }
    }

    private void Update<T>(MemberExpression expression)
    {
        throw new NotImplementedException();
    }

    private void Update<T>(MemberAssignment assignmentExpression)
    {
        var type = assignmentExpression.Expression.GetType();

        if (assignmentExpression.Expression is BinaryExpression expression)
        {
            switch (assignmentExpression.Expression.NodeType)
            {
                case ExpressionType.Add:
                    Builder.UpdateFieldWithOperation(GetColumnName(expression.Left),
                                                     GetExpressionValue(expression.Right),
                                                     "+");
                    break;
                case ExpressionType.Subtract:
                    Builder.UpdateFieldWithOperation(GetColumnName(expression.Left),
                                                     GetExpressionValue(expression.Right),
                                                     "-");
                    break;
                case ExpressionType.Multiply:
                    Builder.UpdateFieldWithOperation(GetColumnName(expression.Left),
                                                     GetExpressionValue(expression.Right),
                                                     "*");
                    break;
                case ExpressionType.Divide:
                    Builder.UpdateFieldWithOperation(GetColumnName(expression.Left),
                                                     GetExpressionValue(expression.Right),
                                                     "/");
                    break;
            }

            return;
        }

        if (assignmentExpression.Expression is UnaryExpression unaryExpression)
        {
            var columnName      = GetColumnName(assignmentExpression);
            var expressionValue = GetExpressionValue(unaryExpression);
            Builder.UpdateAssignField(columnName, expressionValue);

            return;
        }

        if (assignmentExpression.Expression is MethodCallExpression mce)
        {
            ResolveUpdateMethodCall(mce);
        }
    }

    private void ResolveUpdateMethodCall(MethodCallExpression callExpression)
    {
        var arguments = callExpression.Arguments.Select(GetExpressionValue).ToArray();
        var resolver  = StatementResolvers.FirstOrDefault(_ => _.SupportedMethod == callExpression.Method);

        if (resolver == null)
            throw new
                NotSupportedException($"The provided method expression {callExpression.Method.DeclaringType.Name}.{callExpression.Method.Name}() is not supported");

        resolver.ResolveStatement(Builder, callExpression, arguments);
    }
}