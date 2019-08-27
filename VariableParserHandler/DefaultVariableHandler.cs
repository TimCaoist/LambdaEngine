using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.VariableParserHandler
{
    public class DefaultVariableHandler : BaseVariableHandler
    {
        public override string[] Tokens => throw new NotImplementedException();

        public override bool Default => true;

        private const string Alat = "@";

        public override bool IsMatch(string token)
        {
            return false;
        }

        internal override int TryAddVariable(IEnumerable<Token> tokens, Token token, ICollection<Variable> variables, int i)
        {
            var variable = CreateVariable(token.Flag);
            variables.Add(variable);
            return i;
        }

        public static ConstVariable CreateVariable(string flag)
        {
            if (!flag.StartsWith(Alat))
            {
                var variable = new ConstVariable
                {
                    Value = flag
                };

                return variable;
            }

            return new ParamVariable
            {
                Value = flag
            };
        }
    }
}
