using DotNetBrightener.LinQToSqlBuilder.ValueObjects;

namespace DotNetBrightener.LinQToSqlBuilder.Resolver.ExpressionTree;

internal class LikeNode : Node
{
    public LikeMethod Method     { get; set; }
    public MemberNode MemberNode { get; set; }
    public string     Value      { get; set; }
}