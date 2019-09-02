using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.VariableParserHandler
{
    public class IfVariableHandler : BranchVariableHandler
    {
        public override string[] Tokens => new string[] { "if", "else"};

        private const int skipCount = 4;

        internal override int TryAddVariable(IEnumerable<Token> tokens, Token token, ICollection<Variable> variables, int i)
        {
            if (token.Flag == Tokens[0])
            {
                var branch = CreateIfBranchVariable(tokens, token, variables, i);
                variables.Add(branch);
                return i + skipCount + branch.TokenCount;
            }

            var lastVariable = variables.Last();
            if (lastVariable.Type != VariableType.IfBranch)
            {
                throw new ArgumentException("语法错误!");
            }

            IfBranchVariable ifBranchVariable = (IfBranchVariable)lastVariable;
            var nextToken = tokens.ElementAt(i + 1);
            if (nextToken.Flag == Tokens[0])
            {
                var branch = CreateIfBranchVariable(tokens, token, variables, i + 1);
                ifBranchVariable.ElseIf.Add(branch);
                return i + skipCount + branch.TokenCount;
            }

            ifBranchVariable.Else = CreateElseBranchVariable(tokens, token, variables, i);
            return i + 2 + ifBranchVariable.TokenCount;
        }

        public IfBranchVariable CreateElseBranchVariable(IEnumerable<Token> tokens, Token token, ICollection<Variable> variables, int i)
        {
            var branchVariable = new IfBranchVariable
            {
                Value = token.Flag
            };

            ICollection<Token> subTokens = new List<Token>();
            Util.CollectionTokens(tokens, subTokens, i + 1, Strings.StartFlag2, Strings.EndFlag2, true);
            var count = subTokens.Count();

            ICollection<Variable> bodyVariables = new List<Variable>();
            branchVariable.Variables = bodyVariables;

            for (var a = 0; a < count; a++)
            {
                var subToken = subTokens.ElementAt(a);
                var handler = VariableHandleFactory.Create(subToken.Flag);
                a = handler.TryAddVariable(subTokens, subToken, bodyVariables, a);
            }

            branchVariable.TokenCount = count;
            return branchVariable;
        }

        public IfBranchVariable CreateIfBranchVariable(IEnumerable<Token> tokens, Token token, ICollection<Variable> variables, int i)
        {
            var branchVariable = new IfBranchVariable
            {
                Value = token.Flag
            };

            ICollection<Token> subTokens = new List<Token>();
            Util.CollectionTokens(tokens, subTokens, i + 1, Strings.StartFlag1, Strings.EndFlag1, true);
            var count = subTokens.Count();

            branchVariable.TokenCount = count;
            ICollection<Variable> paramVariables = new List<Variable>();
            branchVariable.ParamVariables = paramVariables;

            for (var a = 0; a < count; a ++)
            {
                var subToken = subTokens.ElementAt(a);
                var handler = VariableHandleFactory.Create(subToken.Flag);
                a = handler.TryAddVariable(subTokens, subToken, paramVariables, a);
            }

            subTokens.Clear();

            ICollection<Variable> bodyVariables = new List<Variable>();
            branchVariable.Variables = bodyVariables;
            Util.CollectionTokens(tokens, subTokens, i + 3 + count, Strings.StartFlag2, Strings.EndFlag2, true);
            count = subTokens.Count();

            for (var a = 0; a < count; a++)
            {
                var subToken = subTokens.ElementAt(a);
                var handler = VariableHandleFactory.Create(subToken.Flag);
                a = handler.TryAddVariable(subTokens, subToken, bodyVariables , a);
            }

            branchVariable.TokenCount += count;
            return branchVariable;
        }
    }
}
