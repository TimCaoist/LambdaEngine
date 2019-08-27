﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.LambdaEngine.Models
{
    public class SwitchBranchVariable : BranchVariable
    {
        public override VariableType Type => VariableType.SwitchBranch;

        public ConstVariable Param { get; internal set; }
    }
}
