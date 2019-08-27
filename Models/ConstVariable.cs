using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tim.LambdaEngine.Models
{
    public class ConstVariable : Variable
    {
        public override VariableType Type => VariableType.Const;

        public bool IsParamer { get; set; }

        public Type ValType { get; internal set; }

        internal virtual object GetValue(IDictionary<string, object> datas)
        {
            if (Value == "true" || Value == "false")
            {
                return bool.Parse(Value);
            }

            if (IsInt(Value))
            {
                return int.Parse(Value);
            }

            if (IsNumeric(Value))
            {
                return long.Parse(Value);
            }

            if (Value.StartsWith("'"))
            {
                return char.Parse(Value.Replace("'", string.Empty));
            }

            if (Value.StartsWith("\""))
            {
                return Value.Replace("\"", string.Empty);
            }

            throw new ArgumentException(Value + "未能识别匹配的类型");
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
