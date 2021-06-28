using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class ParamReplacerVisitor : ExpressionVisitor
    {
        private Dictionary<string, int> listOfValues = new Dictionary<string, int>();

        protected override Expression VisitBlock(BlockExpression node)
        {
            if (node.Expressions[0].NodeType == ExpressionType.Lambda && node.Expressions[1].NodeType == ExpressionType.Constant)
            {

                this.listOfValues = ((ConstantExpression)node.Expressions[1]).Value as Dictionary<string, int>;

                return Visit(node.Expressions[0]);
            }

            return base.VisitBlock(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return Visit(node.Body);
        }


        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (listOfValues.ContainsKey(node.Name))
            {
                var value = listOfValues[node.Name];
                return Expression.Constant(value, value.GetType());
            }


            return base.VisitParameter(node);
        }
    }
}