using System.Collections.Generic;
using UnityEngine;

namespace Test.Core.Data
{
    /// <summary>
    /// Describes order and implementation for module system
    /// </summary>
    [CreateAssetMenu(menuName = "Modules/Data/SystemConfig")]
    public class SystemConfig : ScriptableObject
    {
        [SerializeField] private List<TypeBinding> moduleBindings;
        [SerializeField] private List<ScriptableObjectBinding> resourceBindings;
        [SerializeField] private SceneIdentifier worldScene;

        public IReadOnlyCollection<TypeBinding> Modules => moduleBindings;

        public IReadOnlyCollection<ScriptableObjectBinding> Resources => resourceBindings;

        public SceneIdentifier WorldScene => worldScene;
    }
}