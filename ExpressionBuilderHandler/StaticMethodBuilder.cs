using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.ExpressionBuilderHandler
{
    public class StaticMethodBuilder : BaseExpressionBuilder
    {
        protected override VariableType ValueType => VariableType.StaticMethod;

        internal override int BuilderExpression(Context context)
        {
            var variable = (StaticMethodVariable)context.Variable;
            Expression expression = ExpressionBuilder.BuildRealParam(variable, context.ValuePairs, context.Datas, null, variable.Path);
            if (context.Body == null)
            {
                context.Expressions.Add(expression);
            }

            if (context.Index == 0)
            {
                return context.Index;
            }

            var prevVariable = context.Variables.ElementAt(context.Index - 1);
            if (variable.Type == VariableType.Operation)
            {
                return context.Index;
            }

            context.ExcuteParams.Clear();
            context.Expressions.Add(expression);
            context.ExcuteParams.Add(expression);
            return context.Index;
        }
    }
}
