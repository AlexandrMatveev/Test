using System;
using UnityEngine;

namespace Test.Core.Data
{
    /// <summary>
    /// Contains description for generic unity/c# object, that can be used as some gui content
    /// </summary>
    [Serializable]
    public class ObjectDescriptor
    {
        [SerializeField] private string name;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObjectBinding gameObject;

        public string Name => name;
        public string Description => description;
        public Sprite Icon => icon;
        public GameObjectBinding GameObject => gameObject;
    }
}