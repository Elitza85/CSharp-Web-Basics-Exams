using System;
using System.Collections.Generic;
using System.Linq;

namespace SUS.MvcFramework
{
    public class ServiceCollection : IServiceCollection
    {
        private Dictionary<Type, Type> dependencyContainer = new Dictionary<Type, Type>();
        public void Add<TSource, TDestination>()
        {
            this.dependencyContainer[typeof(TSource)] = typeof(TDestination);
        }

        public object CreateInstance(Type type)
        {
            if (this.dependencyContainer.ContainsKey(type))
            {
                type = this.dependencyContainer[type];
            }

            var constructor = type.GetConstructors()
                .OrderBy(x => x.GetParameters().Count())
                .FirstOrDefault();
            var parameters = constructor.GetParameters();
            var paramValues = new List<object>();
            foreach (var parameter in parameters)
            {
                var paramValue = CreateInstance(parameter.ParameterType);
                paramValues.Add(paramValue);
            }
            var obj = constructor.Invoke(paramValues.ToArray());
            return obj;
        }
    }
}
