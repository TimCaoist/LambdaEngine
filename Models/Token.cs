using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.LambdaEngine.Models
{
    public class Token
    {
        public Token(string str, int i)
        {
            this.Flag = str;
            this.EndIndex = i;
        }

        public int EndIndex { get; private set; }

        public string Flag { get; private set; }
    }
}
