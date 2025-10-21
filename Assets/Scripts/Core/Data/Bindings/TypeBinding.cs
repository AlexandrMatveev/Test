using System;
using UnityEditor;
using UnityEngine;

namespace Test.Core.Data
{
    /// <summary>
    /// Describes binding relationships for two type
    /// </summary>
    [Serializable]
    public class TypeBinding : ISerializationCallbackReceiver
    {
        [SerializeField] private string primary;
        [SerializeField] private string secondary;

#if UNITY_EDITOR
        [SerializeField] private MonoScript primaryScript;
        [SerializeField] private MonoScript secondaryScript;
#endif

        public Type Primary { get; private set; }
        public Type Secondary { get; private set; }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (primaryScript)
            {
                Primary = primaryScript.GetClass();
                primary = Primary.AssemblyQualifiedName;
            }

            if (secondaryScript)
            {
                Secondary = secondaryScript.GetClass();
                secondary = Secondary.AssemblyQualifiedName;
            }
#endif
        }

        public void OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(primary)) Primary = Type.GetType(primary);

            if (!string.IsNullOrEmpty(secondary)) Secondary = Type.GetType(secondary);
        }
    }
}