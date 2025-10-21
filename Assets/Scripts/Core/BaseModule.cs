using System;
using System.Collections.Generic;
using System.Linq;
using Test.Core.Abstractions;
using Test.Core.Communication;
using Test.Core.Data;
using UnityEngine;

namespace Test.Core
{
    /// <summary>
    /// Basic class for every module entity
    /// </summary>
    public abstract class BaseModule : IModule
    {
        /// <summary>
        /// Contains all type and resource bindings
        /// </summary>
        protected Container Container { get; private set; }

        /// <summary>
        /// Contains reference for global player data/save data storage
        /// </summary>
        protected PlayerData PlayerData { get; private set; }

        /// <summary>
        /// Scene resources resolver
        /// </summary>
        protected IExposedPropertyTable Resolver { get; private set; }

        /// <summary>
        ///  Global event bus reference
        /// </summary>
        protected EventBus Bus { get; private set; }

        /// <summary>
        /// All event's subscriptions related to this module
        /// </summary>
        protected List<IDisposable> Subscriptions { get; private set; }

        public abstract void InitializeModule();

        public virtual void InstallGlobalDependencies(Container container, PlayerData playerData, EventBus bus)
        {
            Container = container;
            PlayerData = playerData;
            Bus = bus;
        }

        public virtual void InstallLocalDependencies(IExposedPropertyTable resolver)
        {
            Resolver = resolver;
        }

        public void InstallSubscriptions()
        {
            if (Subscriptions == null) Subscriptions = new List<IDisposable>();

            if (Subscriptions.Count > 0)
            {
                foreach (var subscription in Subscriptions) subscription.Dispose();

                Subscriptions.Clear();
            }

            Subscriptions.AddRange(Subscribe(Bus));
        }

        protected virtual IEnumerable<IDisposable> Subscribe(EventBus bus)
        {
            return Enumerable.Empty<IDisposable>();
        }

        public virtual void WorldChanged()
        {
        }
    }
}