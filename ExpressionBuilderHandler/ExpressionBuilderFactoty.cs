using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.ExpressionBuilderHandler
{
    public static class ExpressionBuilderFactoty
    {
        private readonly static ICollection<BaseExpressionBuilder> handles = new List<BaseExpressionBuilder>();

        static ExpressionBuilderFactoty()
        {
            handles.Add(new OperationBuilder());
            handles.Add(new ConstBuilder());
            handles.Add(new SwitchBuilder());
            handles.Add(new IfBuilder());
            handles.Add(new StaticMethodBuilder());
        }

        public static BaseExpressionBuilder Create(Variable variable)
        {
            var handler = handles.First(h => h.IsMatch(variable));
            return handler;
        }

    }
}
