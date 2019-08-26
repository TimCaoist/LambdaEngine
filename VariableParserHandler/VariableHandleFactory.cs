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
            handles.Add(new AssignVariableHandler());
            handles.Add(new DefaultVariableHandler());
            handles.Add(new IngoreVariableHandler());
            handles.Add(new OperationVariableHandler());
            handles.Add(new SwitchVariableHandler());
            handles.Add(new TypeVariableHandler());
        }


        public static BaseVariableHandler Create(string text)
        {
            var handler = handles.FirstOrDefault(h => h.IsMatch(text));
            return handler ?? handles.First(h => h.Default);
        }
    }
}
