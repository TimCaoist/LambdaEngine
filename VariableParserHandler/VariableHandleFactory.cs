using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.LambdaEngine.VariableParserHandler
{
    public static class VariableHandleFactory
    {
        private readonly static ICollection<BaseVariableHandler> handles = new List<BaseVariableHandler>();

        static VariableHandleFactory()
        {
            var handlerTypes = TypeLoader.GetSubTypes(typeof(BaseVariableHandler));
            foreach (var type in handlerTypes)
            {
                BaseVariableHandler variableHandler = (BaseVariableHandler)Activator.CreateInstance(type);
                handles.Add(variableHandler);
            }
        }


        public static BaseVariableHandler Create(string text)
        {
            var handler = handles.FirstOrDefault(h => h.IsMatch(text));
            return handler ?? handles.First(h => h.Default);
        }
    }
}
