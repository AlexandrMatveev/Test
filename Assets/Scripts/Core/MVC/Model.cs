using System;
using Test.Core.Data;

namespace Core.MVC
{
    public abstract class Model
    {
        public event Action<ObjectIdentifier> OnModelChanged;

        protected void NotifyModuleChange(ObjectIdentifier identifier)
        {
            OnModelChanged?.Invoke(identifier);
        }
    }

    /// <summary>
    /// Basic common implementation MVC's model part
    /// </summary>
    public abstract class Model<T, K> : Model, IDisposable
    {
        private T moduleData;
        private K playerData;

        protected T ModuleData => moduleData;

        protected K PlayerData => playerData;

        protected Model(T moduleData, K playerData)
        {
            this.moduleData = moduleData;
            this.playerData = playerData;
        }

        public abstract ObjectIdentifier GetIdentifier();

        public abstract ObjectDescriptor GetDescriptor();

        public virtual void Dispose()
        {
        }
    }
}