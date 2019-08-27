using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.ExpressionBuilderHandler
{
    public class SwitchBuilder : BaseExpressionBuilder
    {
        protected override VariableType ValueType => VariableType.SwitchBranch;

        internal override int BuilderExpression(Context context)
        {
            SwitchBranchVariable switchBranch = (SwitchBranchVariable)context.Variable;
            IEnumerable<CaseVariable> caseVariables = (IEnumerable<CaseVariable>)switchBranch.Variables;
            var cases = caseVariables.Where(v => !v.Default && v.Variables != null && v.Variables.Any()).Select(
                c => {
                    ICollection<Expression> expressions = new List<Expression>();
                    ExpressionBuilder.BuildExpression(expressions, c.Variables, context.ValuePairs, context.Datas);
                    var values = c.ConstVariable.Select(v => ExpressionBuilder.GetExpression(v, context.ValuePairs, context.Datas));
                    return Expression.SwitchCase(Expression.Block(expressions), values);
                }
            ).ToArray();

            var defaultCase = caseVariables.FirstOrDefault(c => c.Default);
            Expression defaultBody = null;
            if (defaultCase != null)
            {
                ICollection<Expression> expressions = new List<Expression>();
                ExpressionBuilder.BuildExpression(expressions, defaultCase.Variables, context.ValuePairs, context.Datas);
                var values = defaultCase.ConstVariable.Select(v => ExpressionBuilder.GetExpression(v, context.ValuePairs, context.Datas));
                defaultBody = Expression.Block(expressions);
            }
            else {
                var firstType = cases.First().Body.Type;
                defaultBody = Expression.Block(firstType, Expression.Constant(Activator.CreateInstance(firstType)));
            }

            var param = ExpressionBuilder.GetExpression(switchBranch.Param, context.ValuePairs, context.Datas);
            SwitchExpression switchExpression = Expression.Switch(param, defaultBody, cases);
            context.Expressions.Add(switchExpression);
            context.ExcuteParams.Clear();

            return context.Index;
        }
    }
}
