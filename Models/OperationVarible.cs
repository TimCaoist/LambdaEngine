using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.LambdaEngine.Models
{
    public class OperationVarible : Variable
    {
        public override VariableType Type => VariableType.Operation;
    }
}
