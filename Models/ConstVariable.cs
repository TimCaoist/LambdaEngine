using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tim.LambdaEngine.Models
{
    public class ConstVariable : InvokeVariable
    {
        public override VariableType Type => VariableType.Const;

        public bool IsParamer { get; set; }

        public Type ValType { get; internal set; }
        public bool NotSelf { get; internal set; }
      
        internal virtual object GetValue(IDictionary<string, object> datas)
        {
            if (Name == "true" || Name == "false")
            {
                return bool.Parse(Name);
            }

            if (IsInt(Name))
            {
                return int.Parse(Name);
            }

            if (IsNumeric(Name))
            {
                return long.Parse(Name);
            }

            if (Name.StartsWith("'"))
            {
                return char.Parse(Name.Replace("'", string.Empty));
            }

            if (Name.StartsWith("\""))
            {
                return Name.Replace("\"", string.Empty);
            }

            throw new ArgumentException(Name + "未能识别匹配的类型");
        }

        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
    }
}
