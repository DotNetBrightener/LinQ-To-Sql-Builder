﻿using System;
using System.Linq.Expressions;
using DotNetBrightener.LinQToSqlBuilder.ValueObjects;

namespace DotNetBrightener.LinQToSqlBuilder.Resolver
{
    /// <summary>
    /// Provides methods to perform resolution to SQL expressions for SELECT, JOIN, ORDER BY query from given lambda expressions
    /// </summary>
    partial class LambdaResolver
    {
        public void Join<T1, T2>(Expression<Func<T1, T2, bool>> expression)
        {
            var joinExpression = GetBinaryExpression(expression.Body);
            var leftExpression = GetMemberExpression(joinExpression.Left);
            var rightExpression = GetMemberExpression(joinExpression.Right);

            Join<T1, T2>(leftExpression, rightExpression);
        }

        public void Join<T1, T2, TKey>(Expression<Func<T1, TKey>> leftExpression, Expression<Func<T1, TKey>> rightExpression)
        {
            Join<T1, T2>(GetMemberExpression(leftExpression.Body), GetMemberExpression(rightExpression.Body));
        }

        public void Join<T1, T2>(MemberExpression leftExpression, MemberExpression rightExpression)
        {
            Builder.Join(GetTableName<T1>(), GetTableName<T2>(), GetColumnName(leftExpression), GetColumnName(rightExpression));
        }

        public void OrderBy<T>(Expression<Func<T, object>> expression, bool desc = false)
        {
            var fieldName = GetColumnName(GetMemberExpression(expression.Body));
            Builder.OrderBy(GetTableName<T>(), fieldName, desc);
        }
        
        public void Select<T>(Expression<Func<T, object>> expression)
        {
            Select<T>(expression.Body);
        }

        private void Select<T>(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Parameter:
                    Builder.Select(GetTableName(expression.Type));
                    break;
                case ExpressionType.Convert:
                case ExpressionType.MemberAccess:
                    Select<T>(GetMemberExpression(expression));
                    break;
                case ExpressionType.New:
                    foreach (MemberExpression memberExp in (expression as NewExpression).Arguments)
                        Select<T>(memberExp);
                    break;
                default:
                    throw new ArgumentException("Invalid expression");
            }           
        }

        private void Select<T>(MemberExpression expression)
        {
            if (expression.Type.IsClass && expression.Type != typeof(String))
                Builder.Select(GetTableName(expression.Type));                            
            else
                Builder.Select(GetTableName<T>(), GetColumnName(expression));            
        }

        public void SelectWithFunction<T>(Expression<Func<T, object>> expression, SelectFunction selectFunction)
        {
            SelectWithFunction<T>(expression.Body, selectFunction);
        }

        private void SelectWithFunction<T>(Expression expression, SelectFunction selectFunction)
        {
            var fieldName = GetColumnName(GetMemberExpression(expression));
            Builder.Select(GetTableName<T>(), fieldName, selectFunction);
        }

        public void SelectWithFunction<T>(SelectFunction selectFunction)
        {
            Builder.Select(selectFunction);
        }

        public void GroupBy<T>(Expression<Func<T, object>> expression)
        {
            GroupBy<T>(GetMemberExpression(expression.Body));
        }

        private void GroupBy<T>(MemberExpression expression)
        {
            var fieldName = GetColumnName(GetMemberExpression(expression));
            Builder.GroupBy(GetTableName<T>(), fieldName);
        }
    }
}
