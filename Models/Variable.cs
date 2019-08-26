using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tim.LambdaEngine.Models
{
    public abstract class Variable
    {
        public abstract VariableType Type { get; }

        public string Value { get; set; }

        public IEnumerable<Variable> Variables { get; set; }
    }
}
