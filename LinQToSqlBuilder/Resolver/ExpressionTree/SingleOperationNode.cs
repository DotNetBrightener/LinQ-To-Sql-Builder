using System.Linq.Expressions;

namespace DotNetBrightener.LinQToSqlBuilder.Resolver.ExpressionTree;

class SingleOperationNode : Node
{
    public ExpressionType Operator { get; set; }
    public Node           Child    { get; set; }
}