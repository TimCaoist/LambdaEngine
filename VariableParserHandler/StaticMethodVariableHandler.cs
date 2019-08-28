using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;
using static Tim.LambdaEngine.LambdaEnginerConfig;

namespace Tim.LambdaEngine.VariableParserHandler
{
    public class StaticMethodVariableHandler : BaseVariableHandler
    {
        public override string[] Tokens => throw new NotImplementedException();

        static StaticMethodVariableHandler()
        {
            RegisterMethodType(Strings.Long, typeof(long));
            RegisterMethodType(Strings.Int, typeof(int));
            RegisterMethodType(Strings.String, typeof(string));
            RegisterMethodType(Strings.Math, typeof(Math));
            RegisterMethodType(Strings.Bool, typeof(bool));
            RegisterMethodType(Strings.DateTime, typeof(DateTime));
        }

        public override bool IsMatch(string token)
        {
            if (token.IndexOf(Strings.Split) < 0)
            {
                return false;
            }

            var array = token.Split(Strings.Split);
            return LambdaEnginerConfig.FindMethodType(array[0]) != null;
        }

        internal override int TryAddVariable(IEnumerable<Token> tokens, Token token, ICollection<Variable> variables, int i)
        {
            var flag = token.Flag;
            var array = flag.Split(Strings.Split);
            var key = array[0];

            var staticMethodVariable = new StaticMethodVariable
            {
                Value = flag,
                Name = key,
                InstanceType = LambdaEnginerConfig.FindMethodType(array[0])
            };

            variables.Add(staticMethodVariable);
            Util.SetInvokeParam(tokens, staticMethodVariable, flag, i);
            return i + staticMethodVariable.ParamTokenCount;
        }
    }
}
