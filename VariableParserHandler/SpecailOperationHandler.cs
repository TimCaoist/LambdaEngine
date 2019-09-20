using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.VariableParserHandler
{
    public class SpecailOperationHandler : BaseVariableHandler
    {
        public override string[] Tokens => new string[] { "&&", "||" };

        internal override int TryAddVariable(IEnumerable<Token> tokens, Token token, ICollection<Variable> variables, int i)
        {
            variables.Add(new SpecailOperationVarible
            {
                Value = token.Flag
            });

            return i;
        }
    }
}
