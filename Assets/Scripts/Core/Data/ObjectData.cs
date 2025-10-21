using System;
using UnityEngine;

namespace Test.Core.Data
{
    /// <summary>
    /// Generic data description
    /// </summary>
    [Serializable]
    public class ObjectData
    {
        [SerializeField] private ObjectIdentifier identifier;
        [SerializeField] private ObjectDescriptor descriptor;
        public ObjectIdentifier Identifier => identifier;
        public ObjectDescriptor Descriptor => descriptor;
    }
}