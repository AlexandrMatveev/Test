using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Test.Core.Data
{
    /// <summary>
    /// Simple persistant universal object identifier
    /// </summary>
    [CreateAssetMenu(menuName = "Modules/Data/ObjectIdentifier")]
    public class ObjectIdentifier : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] protected string id;

        public string ID => id;

        public virtual void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(id))
            {
                var key = string.Concat(DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"), Random.value);
                id = Hash128.Compute(key).ToString();
            }
#endif
        }

        public virtual void OnAfterDeserialize()
        {
        }

        public override int GetHashCode()
        {
            return (id ?? string.Empty).GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            return id == ((ObjectIdentifier)other).id;
        }
    }
}