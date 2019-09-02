using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.VariableParserHandler
{
    public class AssignVariableHandler : BaseVariableHandler
    {
        public override string[] Tokens => new string[] { "="};

        internal override int TryAddVariable(IEnumerable<Token> tokens, Token token, ICollection<Variable> variables, int i)
        {
            var lastVariable = variables.Last();
            if (lastVariable.Type != VariableType.Const)
            {
                throw new ArgumentException("Variable类型错误！");
            }

            variables.Add(new AssignVarible
            {
                Value = token.Flag
            });

            return i;
        }
    }
}
