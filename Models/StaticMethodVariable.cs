using System;

namespace Tim.LambdaEngine.Models
{
    public class StaticMethodVariable : InvokeVariable
    {
        public override VariableType Type => VariableType.StaticMethod;

        public Type InstanceType { get; internal set; }
    }
}
