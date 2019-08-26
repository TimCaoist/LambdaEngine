using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;
using Tim.LambdaEngine.VariableParserHandler;

namespace Tim.LambdaEngine
{
    public static class VariableParser
    {
        internal static IEnumerable<Variable> Parser(CodePiece codePiece)
        {
            var tokens = codePiece.Tokens;
            var len = tokens.Count();
            ICollection<Variable> variables = new List<Variable>();
            for (var i = 0; i < len; i++)
            {
                var token = tokens.ElementAt(i);
                var handler = VariableHandleFactory.Create(token.Flag);
                i = handler.TryAddVariable(tokens, token, variables, i);
            }

            return variables;
        }
    }
}
