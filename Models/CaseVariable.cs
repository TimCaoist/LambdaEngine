﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.LambdaEngine.Models
{
    public class CaseVariable : BranchVariable
    {
        public ICollection<ConstVariable> ConstVariable = new List<ConstVariable>();

        public bool Default { get; set; }

        public override VariableType Type => VariableType.SwitchBranch;
    }
}
