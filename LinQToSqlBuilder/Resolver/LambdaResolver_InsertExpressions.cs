using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using DotNetBrightener.LinQToSqlBuilder.Adapter;
using DotNetBrightener.LinQToSqlBuilder.Resolver.ExpressionTree;

namespace DotNetBrightener.LinQToSqlBuilder.Resolver;

/// <summary>
/// Provides methods to perform resolution to SQL expressions for INSERT INTO query from given lambda expressions
/// </summary>
partial class LambdaResolver
{
    /// <summary>
    /// Prepares an INSERT INTO method which expression to copy values from another table
    /// </summary>
    /// <typeparam name="T">The type of entity that associates to the table to insert record(s) to</typeparam>
    /// <param name="expression">The expression that generates the record to insert</param>
    public void Insert<T>(Expression<Func<T, T>> expression)
    {
        Insert<T>(expression.Body);
    }


    /// <summary>
    /// Prepares an INSERT INTO method which expression to copy values from another table
    /// </summary>
    /// <typeparam name="T">The type of entity that associates to the table to insert record(s) to</typeparam>
    /// <param name="expression">The expression that generates the record(s) to insert</param>
    public void Insert<T>(Expression<Func<T, IEnumerable<T>>> expression)
    {
        Insert<T>(expression.Body);
    }

    /// <summary>
    /// Performs an INSERT INTO method which expression to copy values from another table
    /// </summary>
    /// <typeparam name="TFrom">The type of entity associated to the source table</typeparam>
    /// <typeparam name="TTo">The type of entity associated to the destination table</typeparam>
    /// <param name="expression">The expression of taking values from TFrom and assigning to TTo</param>
    public void Insert<TFrom, TTo>(Expression<Func<TFrom, TTo>> expression)
    {
        Insert<TTo, TFrom>(expression.Body);
    }

    /// <summary>
    /// Append OUTPUT to the insert statement to get the output identity of the inserted record.
    /// </summary>
    /// <typeparam name="TEntity">The type of the inserting entity</typeparam>
    public void OutputInsertIdentity<TEntity>()
    {
        if (Builder.Operation != SqlOperations.Insert)
            throw new InvalidOperationException($"Cannot OUTPUT identity for the SQL statement that is not insert");

        var fieldName = "";

        var keyProp = typeof(TEntity).GetProperties()
                                     .FirstOrDefault(_ => _.GetCustomAttribute<KeyAttribute>() != null);

        if (keyProp == null)
        {
            keyProp = typeof(TEntity).GetProperty("Id");
        }

        fieldName = keyProp?.Name;

        if (!string.IsNullOrEmpty(fieldName))
            Builder.OutputInsertIdentity(fieldName);
    }

    private void Insert<T>(Expression expression)
    {
        var type = expression.GetType();
        switch (expression.NodeType)
        {
            case ExpressionType.MemberInit:
                if (!(expression is MemberInitExpression memberInitExpression))
                    throw new ArgumentException("Invalid expression");

                foreach (var memberBinding in memberInitExpression.Bindings)
                {
                    if (memberBinding is MemberAssignment assignment)
                    {
                        Insert<T>(assignment);
                    }
                }

                break;

            case ExpressionType.NewArrayInit:
                if (!(expression is NewArrayExpression newArrayExpression))
                {
                    throw new ArgumentException($"Invalid expression");
                }

                foreach (var recordInitExpression in newArrayExpression.Expressions)
                {
                    Builder.NextInsertRecord();
                    Insert<T>(recordInitExpression);
                }

                break;

            default:
                throw new ArgumentException("Invalid expression");
        }
    }
        
    private void Insert<T>(MemberAssignment assignmentExpression)
    {
        var type = assignmentExpression.Expression.GetType();

        if (assignmentExpression.Expression is ConstantExpression constantExpression)
        {
            var columnName      = GetColumnName(assignmentExpression);
            var expressionValue = GetExpressionValue(constantExpression);
            Builder.AssignInsertField(columnName, expressionValue);

            return;
        }

        if (assignmentExpression.Expression is UnaryExpression unaryExpression)
        {
            var columnName      = GetColumnName(assignmentExpression);
            var expressionValue = GetExpressionValue(unaryExpression);
            Builder.AssignInsertField(columnName, expressionValue);

            return;
        }

        if (assignmentExpression.Expression is MemberExpression memberExpression)
        {

        }

        else
        {

        }
    }

    private void Insert<TTo, TFrom>(Expression expression)
    {
        switch (expression.NodeType)
        {
            case ExpressionType.MemberInit:
                if (!(expression is MemberInitExpression memberInitExpression))
                    throw new ArgumentException("Invalid expression");

                foreach (var memberBinding in memberInitExpression.Bindings)
                {
                    if (memberBinding is MemberAssignment assignment)
                    {
                        Insert<TTo, TFrom>(assignment);
                    }
                }

                break;

            default:
                throw new ArgumentException("Invalid expression");
        }
    }

    private void Insert<TTo, TFrom>(MemberAssignment assignmentExpression)
    {
        var type = assignmentExpression.Expression.GetType();

        if (assignmentExpression.Expression is ConstantExpression constantExpression)
        {
            var columnName      = GetColumnName(assignmentExpression);
            var expressionValue = GetExpressionValue(constantExpression);
            Builder.AssignInsertField(columnName, expressionValue);

            return;
        }

        if (assignmentExpression.Expression is UnaryExpression unaryExpression)
        {
            var columnName      = GetColumnName(assignmentExpression);
            var expressionValue = GetExpressionValue(unaryExpression);
            Builder.AssignInsertField(columnName, expressionValue);

            return;
        }

        if (assignmentExpression.Expression is MemberExpression memberExpression)
        {
            var columnName = GetColumnName(assignmentExpression);
            var node       = ResolveQuery(memberExpression);
            BuildInsertAssignmentSql(columnName, (dynamic)node);
            return;
        }

        else
        {

        }
    }
        
    void BuildInsertAssignmentSql(string columnName, MemberNode sourceNode)
    {
        Builder.AssignInsertFieldFromSource(columnName, sourceNode.TableName, sourceNode.FieldName, _operationDictionary[ExpressionType.Equal]);
    }

    void BuildInsertAssignmentSql(string columnName, Node sourceNode)
    {
        throw new ArgumentException($"Unsupported resolution of Node type");
    }
}