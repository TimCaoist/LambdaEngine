using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.LambdaEngine.Models
{
    public class IfBranchVariable : BranchVariable
    {
        public override VariableType Type => VariableType.IfBranch;

        public IEnumerable<Variable> ParamVariables { get; set; }

        public int TokenCount { get; set; }

        public ICollection<IfBranchVariable> ElseIf { get; set; } = new List<IfBranchVariable>();

        public IfBranchVariable Else { get; set; }
    }
}
