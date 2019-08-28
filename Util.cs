using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;
using Tim.LambdaEngine.VariableParserHandler;

namespace Tim.LambdaEngine
{
    public static class Util
    {
        public static void CollectionTokens(IEnumerable<Token> tokens, ICollection<Token> subTokens, int i, string startFlag, string endFlag, bool ingoreFlag = false)
        {
            var n = tokens.Count();
            var c = 0;
            for (var a = i; a < n; a++)
            {
                if (tokens.ElementAt(a).Flag == startFlag)
                {
                    c++;
                    if (a == i && ingoreFlag)
                    {
                        continue;
                    }
                }
                else if (tokens.ElementAt(a).Flag == endFlag)
                {
                    c--;
                    if (c == 0)
                    {
                        if (ingoreFlag == false)
                        {
                            subTokens.Add(tokens.ElementAt(a));
                        }

                        break;
                    }
                }

                subTokens.Add(tokens.ElementAt(a));
            }
        }

        public static IEnumerable<Expression> BuildExpression(IEnumerable<Variable> variables, IDictionary<string, Expression> valuePairs, IDictionary<string, object> datas)
        {
            ICollection<Expression> expressions = new List<Expression>();
            ExpressionBuilder.BuildExpression(expressions, variables, valuePairs, datas);
            return expressions;
        }

        public static Tuple<ICollection<IEnumerable<Variable>>, ICollection<Token>> GetParamVariables(IEnumerable<Token> tokens, string flag, int i)
        {
            ICollection<Token> subTokens = new List<Token>();
            CollectionTokens(tokens, subTokens, i + 1, Strings.StartFlag1, Strings.EndFlag1, true);

            var len = subTokens.Count();
            ICollection<IEnumerable<Variable>> variables = new List<IEnumerable<Variable>>();
            ICollection<Variable> subVariables = new List<Variable>();
            for (var si = 0; si < len; si++)
            {
                var token = subTokens.ElementAt(si);
                if (token.Flag == Strings.ParamSplit)
                {
                    variables.Add(subVariables.ToArray());
                    subVariables.Clear();
                    continue;
                }

                var handler = VariableHandleFactory.Create(token.Flag);
                si = handler.TryAddVariable(subTokens, token, subVariables, si);
            }

            if (subVariables.Any())
            {
                variables.Add(subVariables);
            }

            return Tuple.Create(variables, subTokens);
        }
    }
}
