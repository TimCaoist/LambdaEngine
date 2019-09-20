using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.ExpressionBuilderHandler
{
    public class AssignBuilder : BaseExpressionBuilder
    {
        protected override VariableType ValueType => VariableType.Assign;

        internal override int BuilderExpression(Context context)
        {
            Expression body = context.Body;
            var vCount = 0;
            var nextBlockExpression = CollectionNextBlockExpression(context, out vCount);
            body = Expression.Assign(body, nextBlockExpression);
            context.Expressions.Remove(context.Body);
            context.Expressions.Add(body);
            return context.Index + vCount;
        }
    }
}
