using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.ExpressionBuilderHandler
{
    public class IfBuilder : BaseExpressionBuilder
    {
        protected override VariableType ValueType => VariableType.IfBranch;

        private Expression BuilderExpression(IEnumerable<Tuple<BlockExpression, BlockExpression>> tuples, BlockExpression elseExpression, int i = 0)
        {
            var tuple = tuples.ElementAt(i);
            if (i + 1 < tuples.Count())
            {
                return Expression.IfThenElse(tuple.Item1, tuple.Item2, BuilderExpression(tuples, elseExpression, i + 1));
            }

            if (elseExpression == null)
            {
                return Expression.IfThen(tuple.Item1, tuple.Item2);
            }

            return Expression.IfThenElse(tuple.Item1, tuple.Item2, elseExpression);
        }

        internal override int BuilderExpression(Context context)
        {
            IfBranchVariable ifBranch = (IfBranchVariable)context.Variable;
            Expression expression = null;
            ICollection<Tuple<BlockExpression, BlockExpression>> expressions = new List<Tuple<BlockExpression, BlockExpression>>();
            expressions.Add(BuildIfCondition(ifBranch, context));
            for (var i = 0; i < ifBranch.ElseIf.Count(); i++)
            {
                expressions.Add(BuildIfCondition(ifBranch.ElseIf.ElementAt(i), context));
            }

            var elseBranch = ifBranch.Else;
            if (elseBranch == null)
            {
                expression = BuilderExpression(expressions, null);
            }
            else {
                var elseExpression = BuildIfCondition(elseBranch, context).Item2;
                expression = BuilderExpression(expressions, elseExpression);
            }
            
            context.Expressions.Add(expression);
            context.ExcuteParams.Clear();
            return context.Index;
        }

        private Tuple<BlockExpression, BlockExpression> BuildIfCondition(IfBranchVariable ifBranch, Context context)
        {
            IEnumerable<Expression> paramExpressions = Enumerable.Empty<Expression>();
            if (ifBranch.ParamVariables != null && ifBranch.ParamVariables.Any())
            {
                paramExpressions = Util.BuildExpression(ifBranch.ParamVariables, context.ValuePairs, context.Datas);
            }
            
            ICollection<Expression> expressions = new List<Expression>();
            ExpressionBuilder.BuildExpression(expressions, ifBranch.Variables, context.ValuePairs, context.Datas);
            return Tuple.Create(paramExpressions.Any() ? Expression.Block(paramExpressions.ToArray()) : null, Expression.Block(expressions.ToArray()));
        }
    }
}
