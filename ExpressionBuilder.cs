using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.ExpressionBuilderHandler;
using Tim.LambdaEngine.Models;
using static System.Linq.Expressions.Expression;

namespace Tim.LambdaEngine
{
    public static class ExpressionBuilder
    {
        public static Expression CallProperty(InvokeVariable variable, Expression expression, string propertyName)
        {
            if (variable.Type == VariableType.Const)
            {
                return Expression.PropertyOrField(expression, propertyName);
            }

            return Expression.Field(null, ((StaticMethodVariable)variable).InstanceType, propertyName);
        }

        public static Expression CallMethod(InvokeVariable variable, IDictionary<string, Expression> valuePairs, IDictionary<string, object> datas, Expression expression, string methodName)
        {
            var varParams = variable.Params;
            MethodInfo methodInfo = null;
            List<Expression> paramExpressions = new List<Expression>();
            Type[] types;
            if (varParams != null)
            {
                foreach (var param in varParams)
                {
                    var fisrtParam = param.First();
                    if (param.Count() == 1)
                    {
                        paramExpressions.Add(GetExpression(fisrtParam, valuePairs, datas));
                    }
                    else {
                        var expressions = Util.BuildExpression(param, valuePairs, datas);
                        paramExpressions.Add(expressions.First());
                    }
                }

                types = paramExpressions.Select(t => t.Type).ToArray();
            }
            else
            {
                types = new Type[] { };
            }

            if (variable.Type == VariableType.Const)
            {
                methodInfo = expression.Type.GetMethod(methodName, types);
            }
            else {
                methodInfo = ((StaticMethodVariable)variable).InstanceType.GetMethod(methodName, types);
            }

            return Expression.Call(expression, methodInfo, paramExpressions.ToArray());
        }

        public static Expression BuildRealParam(InvokeVariable variable, IDictionary<string, Expression> valuePairs, IDictionary<string, object> datas, Expression expression, string path, int start = 0)
        {
            var index = path.IndexOfAny(new char[] { Strings.Split, char.Parse(Strings.StartFlag1) }, start);
            string val = string.Empty;
            if (index < 0)
            {
                val = path.Substring(start);
                return CallProperty(variable, expression, val);
            }

            val = path.Substring(start, index - start);
            if (path[index] == Strings.Split)
            {
                expression = CallProperty(variable, expression, val);
            }
            else {
                return CallMethod(variable, valuePairs, datas, expression, val);
            }

            if (index > -1)
            {
                return BuildRealParam(variable, valuePairs, datas, expression, path, index + 1);
            }

            return expression;
        }

        public static Expression GetExpression(Variable variable, IDictionary<string, Expression> valuePairs, IDictionary<string, object> datas)
        {
            if (variable.Type == VariableType.StaticMethod)
            {
                InvokeVariable invokeVariable = (InvokeVariable)variable;
                return BuildRealParam(invokeVariable, valuePairs, datas, null, invokeVariable.Path);
            }

            Expression expression;
            ConstVariable constVariable = (ConstVariable)variable;
            if (!valuePairs.TryGetValue(constVariable.Name, out expression))
            {
                if (constVariable.IsParamer == false)
                {
                    expression = Expression.Constant(constVariable.GetValue(datas));
                }
                else {
                    expression = Expression.Parameter(constVariable.ValType, constVariable.Name);
                }

                valuePairs.Add(constVariable.Name, expression);
            }

            if (!constVariable.NotSelf)
            {
                return expression;
            }

            return BuildRealParam(constVariable, valuePairs, datas, expression, constVariable.Path);
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
