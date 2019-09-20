using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.LambdaEngine
{
    public static class TypeLoader
    {
        public static IEnumerable<Type> GetSubTypes(Type type)
        {
            var types = type.Assembly.GetTypes();
            ICollection<Type> returnTypes = new List<Type>();
            foreach (var item in types)
            {
                if (item.IsAbstract)
                {
                    continue;
                }

                if (type.IsAbstract && type.IsInterface == false)
                {
                    if (!item.IsSubclassOf(type))
                    {
                        continue;
                    }
                }
                else
                {
                    if (item.GetInterface(type.FullName) == null)
                    {
                        continue;
                    }
                }

                returnTypes.Add(item);
            }

            return returnTypes;
        }
    }
}
