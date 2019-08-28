using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.VariableParserHandler
{
    public class IngoreVariableHandler : BaseVariableHandler
    {
        public override string[] Tokens => new string[] { ";", "(", ")", "{" , "}", Strings.ParamSplit};

        internal override int TryAddVariable(IEnumerable<Token> tokens, Token token, ICollection<Variable> variables, int i)
        {
            return i;
        }
    }
}
