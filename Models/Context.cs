using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tim.LambdaEngine.Models
{
    public class Context
    {
        public int Index { get; internal set; }
        public Variable Variable { get; internal set; }
        public Expression Body {
            get {
                if (!Expressions.Any())
                {
                    return null;
                }

                return Expressions.Last();
            }
        }

        public ICollection<Expression> ExcuteParams { get; internal set; }
        public ICollection<Expression> Expressions { get; set; }
        public IEnumerable<Variable> Variables { get; internal set; }
        public IDictionary<string, object> Datas { get; internal set; }
        public IDictionary<string, Expression> ValuePairs { get; internal set; }
    }
}
