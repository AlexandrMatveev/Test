using System;
using System.Collections.Generic;
using Test.Core.Abstractions;
using Test.Core.Communication;
using Test.Core.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using Bindings = System.Collections.Generic.IEnumerable<Test.Core.Data.TypeBinding>;

namespace Test.Core.Boot
{
    /// <summary>
    /// Entry point for our loading process
    /// </summary>
    public class SystemInitializer : MonoBehaviour
    {
        [SerializeField] private SystemConfig systemConfig;

        private Container container;
        private PlayerData playerData;
        private EventBus bus;

        /// <summary>
        /// We don't have any other entry point,
        /// so for simplification lets use this variant of singleton
        /// </summary>
        private static SystemInitializer Instance { get; set; }

        private void Awake()
        {
            // We don't want to have double initialization
            // So lets have only one global instance
            if (Instance) return;

            DontDestroyOnLoad(this);
            Instance = this;

            try
            {
                RunModuleSystem();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Module system initialization error", e);
            }

            SceneManager.activeSceneChanged += OnSceneLoadedHandler;
            LoadWorld();
        }

        /// <summary>
        /// Creates and bind modules and resources, run initialization code
        /// </summary>
        private void RunModuleSystem()
        {
            if (!systemConfig) throw new InvalidOperationException("Missing boot config. Module system can't start properly!");

            container = new Container();
            playerData = new PlayerData();
            bus = new EventBus();

            var modules = systemConfig.Modules;
            var resources = systemConfig.Resources;
            BindModules(modules);
            BindScriptableObjects(resources);
            InstallGlobalDependencies(modules);
            InstallLocalDependencies(modules);
            InitializeModules(modules);
            InstallSubscriptions(modules);
        }

        /// <summary>
        /// Modules creation and binding
        /// </summary>
        /// <param name="modules"></param>
        private void BindModules(Bindings modules)
        {
            foreach (var module in modules) container.Bind(module.Primary, Activator.CreateInstance(module.Secondary));
        }

        /// <summary>
        /// Resources creation and binding
        /// </summary>
        /// <param name="bindings"></param>
        private void BindScriptableObjects(IEnumerable<ScriptableObjectBinding> bindings)
        {
            foreach (var binding in bindings) container.Bind(binding.Type, binding.Data);
        }

        /// <summary>
        /// Fire module's InstallDependencies callback
        /// </summary>
        private void InstallGlobalDependencies(Bindings modules)
        {
            FireModuleCallback(modules, m => m.InstallGlobalDependencies(container, playerData, bus));
        }

        /// <summary>
        /// Fire module's InitializeModules callback
        /// </summary>
        private void InitializeModules(Bindings modules)
        {
            FireModuleCallback(modules, m => m.InitializeModule());
        }

        /// <summary>
        /// Fire module's InstallLocalDependencies callback
        /// </summary>
        private void InstallLocalDependencies(Bindings modules)
        {
            FireModuleCallback(modules, m => m.InstallLocalDependencies(GameObjectBridge.Instance));
        }

        /// <summary>
        /// Fire module's InstallSubscriptions callback
        /// </summary>
        private void InstallSubscriptions(Bindings modules)
        {
            FireModuleCallback(modules, m => m.InstallSubscriptions());
        }

        /// <summary>
        /// Fire module's FireWorldChange callback
        /// </summary>
        private void FireWorldChange(Bindings modules)
        {
            FireModuleCallback(modules, m => m.WorldChanged());
        }

        /// <summary>
        /// Common mechanize for module callback invocation 
        /// </summary>
        private void FireModuleCallback(Bindings modules, Action<IModule> callback)
        {
            foreach (var module in modules)
            {
                var instance = (IModule)container.Resolve(module.Primary);
                callback(instance);
            }
        }

        /// <summary>
        /// Notify modules whe scene has changed, useful for scene context situations ( bindings, resources, rtc )
        /// </summary>
        private void OnSceneLoadedHandler(Scene oldScene, Scene newScene)
        {
            var modules = systemConfig.Modules;
            InstallLocalDependencies(modules);
            InstallSubscriptions(modules);
            FireWorldChange(modules);
        }

        /// <summary>
        /// Load main scene
        /// </summary>
        private void LoadWorld()
        {
            if (SceneManager.GetActiveScene().path != systemConfig.WorldScene.Path)
                SceneManager.LoadScene(systemConfig.WorldScene.Path, LoadSceneMode.Single);
            else
                FireWorldChange(systemConfig.Modules);
        }
    }
}