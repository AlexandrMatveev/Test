using Test.Core.Communication;
using Test.Core.Data;
using UnityEngine;

namespace Test.Core.Abstractions
{
    /// <summary>
    /// Basic contract for any module entity
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// General method for module initialization, call once on system start
        /// </summary>
        void InitializeModule();

        /// <summary>
        /// Inject of all global dependencies that module need, fires once on system start
        /// </summary>
        void InstallGlobalDependencies(Container container, PlayerData playerData, EventBus bus);

        /// <summary>
        /// Inject of all local/scene related dependencies that module need, fires every scene change
        /// </summary>
        void InstallLocalDependencies(IExposedPropertyTable resolver);

        /// <summary>
        /// Subscribe module for event bus events, fires every scene change
        /// </summary>
        void InstallSubscriptions();

        /// <summary>
        /// Scene change notification, fires every scene change
        /// </summary>
        void WorldChanged();
    }
}