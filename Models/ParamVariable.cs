using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.LambdaEngine.Models
{
    public class ParamVariable : ConstVariable
    {
        private string key;
        public string Key {
            get {
                if (string.IsNullOrEmpty(key))
                {
                    key = Value.Substring(1);
                }

                return key;
            }
        }

        internal override object GetValue(IDictionary<string, object> datas)
        {
            return datas[Key];
        }
    }
}
