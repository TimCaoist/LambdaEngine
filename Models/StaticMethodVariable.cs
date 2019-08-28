using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.LambdaEngine.Models
{
    public class StaticMethodVariable : InvokeVariable
    {
        public override VariableType Type => VariableType.StaticMethod;

        public Type InstanceType { get; internal set; }
    }
}
