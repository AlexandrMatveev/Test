using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Test.Core.Data
{
    /// <summary>
    /// Describes binding tuple for unity object 
    /// </summary>
    [Serializable]
    public class GameObjectBinding : ISerializationCallbackReceiver
    {
        [SerializeField] private SceneIdentifier owner;
        [SerializeField] private PropertyName property;
        [SerializeField] private Object value;

        private bool valid;

        public PropertyName Property => property;

        public Object Value => value;

        public bool Valid()
        {
            return SceneManager.GetActiveScene().path == owner.Path;
        }

        public T Resolve<T>(IExposedPropertyTable resolver) where T : Object
        {
            if (resolver != null)
            {
                var referenceValue = resolver.GetReferenceValue(property, out var idValid);
                if (idValid)
                    return referenceValue as T;
            }

            return value as T;
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (Value)
            {
                var transform = Value switch
                {
                    Transform t => t,
                    Component c => c.transform,
                    GameObject g => g.transform,
                    _ => throw new ArgumentOutOfRangeException()
                };

                var path = TraverseHierarchy(transform).Aggregate(transform.name, (path, next) => Path.Join(next.name, path));
                if (UnityEditor.EditorUtility.IsPersistent(Value)) path = Path.Join("Persistent", path);

                property = path;
            }
#endif

            IEnumerable<Transform> TraverseHierarchy(Transform t)
            {
                while (t.parent != null)
                {
                    t = t.parent;
                    yield return t;
                }
            }
        }

        public void OnAfterDeserialize()
        {
        }
    }
}