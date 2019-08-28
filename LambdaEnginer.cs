using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.LambdaEngine
{
    public static class LambdaEnginer
    {
        public static object Eval(string expression, IDictionary<string, object> excuteParams = null)
        {
            var codePiece = CodeParser.Parser(expression);
            var @delegate = ExpressionBuilder.Build(codePiece, excuteParams);
            return @delegate.DynamicInvoke();
        }
    }
}
