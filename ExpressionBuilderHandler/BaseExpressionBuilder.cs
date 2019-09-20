using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.ExpressionBuilderHandler
{
    public abstract class BaseExpressionBuilder
    {
        protected abstract VariableType ValueType { get;}

        public virtual bool IsMatch(Variable variable)
        {
            return ValueType == variable.Type;
        }

        internal abstract int BuilderExpression(Context context);

        protected static BlockExpression CollectionNextBlockExpression(Context context, out int variableCount)
        {
            var assignIndex = context.Index;
            ICollection<Variable> variables = new List<Variable>();
            var vCount = context.Variables.Count();
            var parentVariables = context.Variables;
            for (var i = assignIndex + 1; i < vCount; i++)
            {
                var current = parentVariables.ElementAt(i);
                if (current is BranchVariable)
                {
                    break;
                }

                if (current.Type == VariableType.Const)
                {
                    if (variables.Any() && variables.Last().Type == VariableType.Const)
                    {
                        break;
                    }
                }

                variables.Add(current);
            }

            ICollection<Expression> expressions = new List<Expression>();
            ExpressionBuilder.BuildExpression(expressions, variables, context.ValuePairs, context.Datas);
            variableCount = variables.Count();
            return Expression.Block(expressions);
        }
    }
}
