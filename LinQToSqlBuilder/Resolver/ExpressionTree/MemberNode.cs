namespace DotNetBrightener.LinQToSqlBuilder.Resolver.ExpressionTree;

internal class MemberNode : Node
{
    public string TableName { get; set; }
    public string FieldName { get; set; }
}