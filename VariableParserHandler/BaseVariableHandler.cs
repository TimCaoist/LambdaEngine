using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.VariableParserHandler
{
    public abstract class BaseVariableHandler
    {
        public BaseVariableHandler() {
        }

        public virtual bool Default { get; }

        public abstract string[] Tokens
        {
            get;
        }

        public virtual bool IsMatch(string token)
        {
            return Tokens.Contains(token);
        }

        internal abstract int TryAddVariable(IEnumerable<Token> tokens, Token token, ICollection<Variable> variables, int i);
    }
}
