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
            var variable = CreateVariable(tokens, token.Flag, i);
            variables.Add(variable.Item1);
            return variable.Item2;
        }

        public static Tuple<ConstVariable, int> CreateVariable(IEnumerable<Token> tokens, string flag, int i)
        {
            ConstVariable variable = null;
            var array = flag.Split(Strings.Split);
            var key = array[0];
            if (!key.StartsWith(Alat))
            {
                variable = new ConstVariable
                {
                    Value = flag,
                    Name = key,
                };
            }
            else {
                variable = new ParamVariable
                {
                    Value = flag,
                    Name = key,
                };
            }

            if (array.Length == 1)
            {
                return Tuple.Create<ConstVariable, int>(variable, i);
            }

            variable.NotSelf = true;
            var nextToken = tokens.ElementAt(i + 1);
            variable.Path = variable.Value.Remove(0, variable.Name.Count() + 1);
            if (nextToken.Flag != Strings.StartFlag1)
            {
                return Tuple.Create<ConstVariable, int>(variable, i);
            }

            ICollection<Token> subTokens = new List<Token>();
            BranchVariableHandler.CollectionTokens(tokens, subTokens, i + 1, Strings.StartFlag1, Strings.EndFlag1, true);
            variable.Value = string.Concat(flag, Strings.StartFlag1, string.Join(string.Empty, subTokens.Select(t => t.Flag)), Strings.EndFlag1);
            variable.Path = variable.Value.Remove(0, variable.Name.Count() + 1);

            var len = subTokens.Count();
            ICollection<Variable> subVariables = new List<Variable>();
            for (var si = 0; si < len; si++)
            {
                var token = subTokens.ElementAt(si);
                var handler = VariableHandleFactory.Create(token.Flag);
                si = handler.TryAddVariable(subTokens, token, subVariables, si);
            }

            if (subVariables.Any(s => s.Type != VariableType.Const))
            {
                throw new ArgumentException(string.Concat(variable.Path, "语法错误!"));
            }

            variable.Params = subVariables;
            return Tuple.Create<ConstVariable, int>(variable, i);
        }
    }
}
