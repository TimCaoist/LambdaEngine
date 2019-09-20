using System.Linq.Expressions;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.ExpressionBuilderHandler
{
    public class SpecailBuilder : BaseExpressionBuilder
    {
        protected override VariableType ValueType =>  VariableType.SpecialOperationVariable;

        internal override int BuilderExpression(Context context)
        {
            Expression body = context.Body;
            var vCount = 0;
            var nextBlockExpression = CollectionNextBlockExpression(context, out vCount);
            var opreation = context.Variable.Value;
            switch (opreation)
            {
                case "||":
                    body = Expression.OrElse(body, nextBlockExpression);
                    break;
                case "&&":
                    body = Expression.AndAlso(body, nextBlockExpression);
                    break;
            }

            context.Expressions.Remove(context.Body);
            context.Expressions.Add(body);
            return context.Index + vCount;
        }
    }
}
