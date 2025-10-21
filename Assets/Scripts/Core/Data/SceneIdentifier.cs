#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Test.Core.Data
{
    /// <summary>
    /// Make possible use scene with serializable fields  
    /// </summary>
    [CreateAssetMenu(menuName = "Modules/Data/SceneIdentifier")]
    public class SceneIdentifier : ObjectIdentifier
    {
#if UNITY_EDITOR
        [SerializeField] private SceneAsset sceneAsset;
#endif

        public string Path => id;

        public override void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (sceneAsset) id = AssetDatabase.GetAssetPath(sceneAsset);
#endif
        }
    }
}