﻿using System;
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

            var result = Util.GetParamVariables(tokens, flag, i);
            variable.Params = result.Item1;
            variable.Value = string.Concat(flag, Strings.StartFlag1, string.Join(string.Empty, result.Item2.Select(t => t.Flag)), Strings.EndFlag1);
            variable.Path = variable.Value.Remove(0, variable.Name.Count() + 1);
            return Tuple.Create<ConstVariable, int>(variable, i);
        }
    }
}
