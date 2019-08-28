using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.LambdaEngine.Models
{
    public abstract class InvokeVariable : Variable
    {
        public string Path { get; internal set; }

        public IEnumerable<IEnumerable<Variable>> Params { get; internal set; }

        public string Name { get; internal set; }
        public int ParamTokenCount { get; internal set; }
    }
}
