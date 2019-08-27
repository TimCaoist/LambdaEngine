using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.VariableParserHandler
{
    public class SwitchVariableHandler : BranchVariableHandler
    {
        public override string[] Tokens => new string[] { "switch"};

        private const string Break = "break";

        private const string Case = "case";

        private const string DefaultStr = "default";

        internal override int TryAddVariable(IEnumerable<Token> tokens, Token token, ICollection<Variable> variables, int i)
        {
            var branchVariable = new SwitchBranchVariable
            {
                Value = token.Flag
            };

            variables.Add(branchVariable);

            var nextToken = tokens.ElementAt(i + 1);
            if (nextToken.Flag != "(")
            {
                throw new ArgumentException("switch");
            }

            var paramToken = tokens.ElementAt(i + 2);
            var variable = new ConstVariable
            {
                Value = paramToken.Flag
            };

            branchVariable.Param = variable;

            var nextCloseToken = tokens.ElementAt(i + 3);
            if (nextCloseToken.Flag != ")")
            {
                throw new ArgumentException("switch");
            }

            var n = tokens.Count();
            ICollection<Token> subTokens = new List<Token>();
            CollectionTokens(tokens, subTokens, i + 4, "{", "}");
            branchVariable.Variables = GetCaseVariables(subTokens);
            return i + 3 + subTokens.Count();
        }

        private static IEnumerable<CaseVariable> GetCaseVariables(ICollection<Token> tokens)
        {
            var n = tokens.Count();
            ICollection<CaseVariable> caseVariables = new List<CaseVariable>();
            ICollection<Variable> variables = new List<Variable>();
            CaseVariable lastCaseVariable = null;
            for (var a = 0; a < n; a++)
            {
                var token = tokens.ElementAt(a);
                var flag = token.Flag;
                if (flag == Break)
                {
                    lastCaseVariable.Variables = variables.ToArray();
                    variables.Clear();
                    lastCaseVariable = null;
                    continue;
                }
                else if (flag != Case && flag != DefaultStr)
                {
                    var handler = VariableHandleFactory.Create(flag);
                    a = handler.TryAddVariable(tokens, token, variables, a);
                    continue;
                }

                if (lastCaseVariable == null)
                {
                    lastCaseVariable = new CaseVariable();
                    caseVariables.Add(lastCaseVariable);
                }

                if (flag == DefaultStr)
                {
                    lastCaseVariable.Default = true;
                    if (tokens.ElementAt(a + 1).Flag != ":")
                    {
                        throw new ArgumentException("switch 语法错误");
                    }

                    a += 1;
                    continue;
                }

                var nextFlag = tokens.ElementAt(a + 1).Flag;
                lastCaseVariable.ConstVariable.Add(DefaultVariableHandler.CreateVariable(nextFlag));

                var nextSplitFlag = tokens.ElementAt(a + 2).Flag;
                if (nextSplitFlag != ":")
                {
                    throw new ArgumentException("switch 语法错误");
                }

                a += 2;
            }

            return caseVariables;
        }
    }
}
