using System;
using System.Collections.Generic;

namespace Test.Core.Data
{
    /// <summary>
    /// Mega simple implementation of DI
    /// </summary>
    public class Container
    {
        private IDictionary<Type, object> storage = new Dictionary<Type, object>();

        public void Bind(Type type, object implementation)
        {
            storage[type] = implementation;
        }

        public bool HasBinding(Type type)
        {
            return storage.ContainsKey(type);
        }

        public object Resolve(Type type)
        {
            if (storage.TryGetValue(type, out var obj)) return obj;

            return null;
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }
    }
}