using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.ExpressionBuilderHandler
{
    public class ConstBuilder : BaseExpressionBuilder
    {
        protected override VariableType ValueType => VariableType.Const;

        internal override int BuilderExpression(Context context)
        {
            Expression constantExpression = ExpressionBuilder.GetExpression(context.Variable, context.ValuePairs, context.Datas);
            if (context.Body == null)
            {
                context.Expressions.Add(constantExpression);
            }

            context.ExcuteParams.Add(constantExpression);
            if (context.Index == 0)
            {
                return context.Index;
            }

            var variable = context.Variables.ElementAt(context.Index - 1);
            if (variable.Type != VariableType.Const && !(variable is BranchVariable))
            {
                return context.Index;
            }

            context.ExcuteParams.Clear();
            context.Expressions.Add(constantExpression);
            context.ExcuteParams.Add(constantExpression);
            return context.Index;
        }
    }
}
