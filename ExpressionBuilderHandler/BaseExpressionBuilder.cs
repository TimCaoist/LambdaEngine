using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.ExpressionBuilderHandler
{
    public abstract class BaseExpressionBuilder
    {
        protected abstract VariableType ValueType { get;}

        public virtual bool IsMatch(Variable variable)
        {
            return ValueType == variable.Type;
        }

        internal abstract int BuilderExpression(Context context);
    }
}
