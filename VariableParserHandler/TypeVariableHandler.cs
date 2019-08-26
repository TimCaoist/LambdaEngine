using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.VariableParserHandler
{
    public class TypeVariableHandler : BaseVariableHandler
    {
        public override string[] Tokens => new string[] { "int", "Int32", "string", "String", "long", "Int64", "DateTime", "char" };

        internal override int TryAddVariable(IEnumerable<Token> tokens, Token token, ICollection<Variable> variables, int i)
        {
            Type type = GetType(token.Flag);
            var variable = new ConstVariable
            {
                Value = tokens.ElementAt(i + 1).Flag,
                IsParamer = true,
                ValType = type
            };

            variables.Add(variable);
            return i + 1;
        }

        private static Type GetType(string flag)
        {
            switch (flag)
            {
                case "int":
                case "Int32":
                    return typeof(int);
                case "string":
                case "String":
                    return typeof(string);
                case "long":
                case "Int64":
                    return typeof(long);
                case "DateTime":
                    return typeof(DateTime);
                case "char":
                    return typeof(char);
            }

            return null;
        }
    }
}
