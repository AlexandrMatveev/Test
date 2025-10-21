using System;
using UnityEditor;
using UnityEngine;

namespace Test.Core.Data
{
    /// <summary>
    /// Binding for specific type and SO 
    /// </summary>
    [Serializable]
    public class ScriptableObjectBinding : ISerializationCallbackReceiver
    {
        [SerializeField] private string type;
        [SerializeField] private ScriptableObject data;

        public Type Type { get; private set; }
        public ScriptableObject Data => data;


#if UNITY_EDITOR
        [SerializeField] private MonoScript script;
#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (script)
            {
                Type = script.GetClass();
                type = Type.AssemblyQualifiedName;
            }
#endif
        }

        public void OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(type)) Type = Type.GetType(type);
        }
    }
}