﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tim.LambdaEngine.Models;

namespace Tim.LambdaEngine.ExpressionBuilderHandler
{
    public static class ExpressionBuilderFactoty
    {
        private readonly static ICollection<BaseExpressionBuilder> handles = new List<BaseExpressionBuilder>();

        static ExpressionBuilderFactoty()
        {
            var handlerTypes = TypeLoader.GetSubTypes(typeof(BaseExpressionBuilder));
            foreach (var type in handlerTypes)
            {
                BaseExpressionBuilder variableHandler = (BaseExpressionBuilder)Activator.CreateInstance(type);
                handles.Add(variableHandler);
            }
        }

        public static BaseExpressionBuilder Create(Variable variable)
        {
            var handler = handles.First(h => h.IsMatch(variable));
            return handler;
        }

    }
}
