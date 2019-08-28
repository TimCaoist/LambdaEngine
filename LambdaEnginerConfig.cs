using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.LambdaEngine
{
    public static class LambdaEnginerConfig
    {
        private static Dictionary<string, Type> staticMethodTypes = new Dictionary<string, Type>();

        public static void RegisterMethodType(string name, Type type)
        {
            staticMethodTypes.Add(name, type);
        }

        public static Type FindMethodType(string name)
        {
            Type type = null;
            staticMethodTypes.TryGetValue(name, out type);
            return type;
        }
    }
}
