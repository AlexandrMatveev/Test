using System.Collections.Generic;
using Test.Core.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Test.Core
{
    /// <summary>
    /// Because we can't use high level GUI system we need some bridge to connect our GUI with world GUI
    /// Lets use scene->so bindings for view's
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class GameObjectBridge : MonoBehaviour, IExposedPropertyTable, ISerializationCallbackReceiver
    {
        [SerializeField] private List<GameObjectBinding> bindings;

        /// <summary>
        /// We don't have any other entry point,
        /// so for simplification lets use this variant of singleton
        /// </summary>
        public static GameObjectBridge Instance { get; private set; }

        private Dictionary<PropertyName, Object> Bindings { get; } = new();

        private void Awake()
        {
            Instance = this;
        }

        public bool Contains(Object obj)
        {
            return Bindings.ContainsValue(obj);
        }

        public void SetReferenceValue(PropertyName id, Object value)
        {
            Bindings[id] = value;
        }

        public void ClearReferenceValue(PropertyName id)
        {
            Bindings.Remove(id);
        }

        public Object GetReferenceValue(PropertyName id, out bool idValid)
        {
            if (Bindings.TryGetValue(id, out var value))
            {
                idValid = true;
                return value;
            }

            idValid = false;
            return null;
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            Bindings.Clear();
            foreach (var item in bindings) Bindings.Add(item.Property, item.Value);
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}