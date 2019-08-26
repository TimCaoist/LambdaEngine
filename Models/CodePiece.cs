using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.LambdaEngine.Models
{
    public class CodePiece
    {
        public ICollection<Token> Tokens { get; set; } = new List<Token>();

        public IEnumerable<Variable> Variables { get; set; }

    }
}
