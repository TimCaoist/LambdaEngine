using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.ExpressionBuilderHandler;
using Tim.LambdaEngine.Models;
using static System.Linq.Expressions.Expression;

namespace Tim.LambdaEngine
{
    public static class ExpressionBuilder
    {
        public static Expression GetExpression(Variable variable, IDictionary<string, Expression> valuePairs, IDictionary<string, object> datas)
        {
            Expression expression;
            if (!valuePairs.TryGetValue(variable.Value, out expression))
            {
                var cVariable = (ConstVariable)variable;
                if (cVariable.IsParamer == false)
                {
                    expression = Expression.Constant(cVariable.GetValue(datas));
                }
                else {
                    expression = Expression.Parameter(cVariable.ValType, variable.Value);
                }

                valuePairs.Add(variable.Value, expression);
            }

            return expression;
        }

        public static void BuildExpression(ICollection<Expression> expressions, IEnumerable<Variable> variables, IDictionary<string, Expression> valuePairs, IDictionary<string, object> datas)
        {
            var count = variables.Count();
            ICollection<Expression> excuteParams = new List<Expression>();
            Context context = new Context
            {
                ExcuteParams = excuteParams,
                Expressions = expressions,
                Variables = variables,
                Datas = datas,
                ValuePairs = valuePairs
            };

            for (var i = 0; i < count; i++)
            {
                var variable = variables.ElementAt(i);
                var handler = ExpressionBuilderFactoty.Create(variable);
                context.Index = i;
                context.Variable = variable;
                i = handler.BuilderExpression(context);
            }
        }

        public static Delegate Build(CodePiece codePiece, IDictionary<string, object> datas)
        {
            IDictionary<string, Expression> valuePairs = new Dictionary<string, Expression>();
            ICollection<Expression> expressions = new List<Expression>();
            BuildExpression(expressions, codePiece.Variables, valuePairs, datas);
            var pEx = valuePairs.Values.Where(v => v is ParameterExpression).Cast<ParameterExpression>();
            return Expression.Lambda(Expression.Block(pEx.ToArray(), expressions)).Compile();
        }
    }
}
