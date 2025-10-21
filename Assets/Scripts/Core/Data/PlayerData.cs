using System;
using System.Collections.Generic;
using Test.Core.Abstractions;

namespace Test.Core.Data
{
    /// <summary>
    /// Basic save system implemetation
    /// It make sence to imp this as core module
    /// but anyway this out of the scope
    /// </summary>
    public class PlayerData
    {
        private IDictionary<Type, object> storage;

        public PlayerData()
        {
            storage = new Dictionary<Type, object>();
        }

        public PlayerData(IDictionary<Type, object> dataSource)
        {
            storage = new Dictionary<Type, object>(dataSource);
        }

        public object GetData<T>() where T : IModule
        {
            if (storage.TryGetValue(typeof(T), out var data)) return data;

            return default(T);
        }

        public K GetData<T, K>() where T : IModule
        {
            return (K)GetData<T>();
        }

        public void SetData<T>(object data) where T : IModule
        {
            storage[typeof(T)] = data;
        }
    }
}