using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;
using static System.Linq.Expressions.Expression;

namespace Tim.LambdaEngine.ExpressionBuilderHandler
{
    public class OperationBuilder : BaseExpressionBuilder
    {
        protected override VariableType ValueType => VariableType.Operation;

        internal override int BuilderExpression(Context context)
        {
            var body = context.Body;
            var opreation = context.Variable.Value;
            var variable = context.Variables.ElementAt(context.Index + 1);
            var skip = 1;
            var subOpreation = string.Empty;
            if (variable.Type != VariableType.Const)
            {
                if (variable.Type != VariableType.Operation)
                {
                    throw new ArgumentException("语法错误");
                }

                subOpreation = variable.Value;
                variable = context.Variables.ElementAt(context.Index + 2);
                skip = 2;
                if (variable.Type != VariableType.Const)
                {
                    throw new ArgumentException("语法错误");
                }
            }

            Expression constantExpression = ExpressionBuilder.GetExpression(variable, context.ValuePairs, context.Datas);
            switch (subOpreation)
            {
                case "-":
                    constantExpression = Expression.Negate(constantExpression);
                    break;
                case "!":
                    constantExpression = Expression.Not(constantExpression);
                    break;
                case "":
                    break;
                default:
                    throw new ArgumentException("不支持该运算" + subOpreation);
            }

            switch (opreation)
            {
                case "+":
                    body = Add(body, constantExpression);
                    break;
                case "-":
                    body = Subtract(body, constantExpression);
                    break;
                case "*":
                    body = Multiply(body, constantExpression);
                    break;
                case "/":
                    body = Divide(body, constantExpression);
                    break;
                case "%":
                    body = Modulo(body, constantExpression);
                    break;
                case "<=":
                    body = LessThanOrEqual(body, constantExpression);
                    break;
                case ">=":
                    body = GreaterThanOrEqual(body, constantExpression);
                    break;
                case "<":
                    body = LessThan(body, constantExpression);
                    break;
                case ">":
                    body = GreaterThan(body, constantExpression);
                    break;
                case "=":
                    body = Assign(body, constantExpression);
                    break;
                case "==":
                    body = Equal(body, constantExpression);
                    break;
                    //case "!":
                    //    body = Negate(constantExpression);
                    //    break;
            }

            context.Expressions.Remove(context.Body);
            context.Expressions.Add(body);
            return context.Index + skip;
        }
    }
}
