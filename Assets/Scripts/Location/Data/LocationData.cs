using System;
using Test.Core.Data;
using UnityEngine;

namespace Test.Location.Data
{
    [Serializable]
    public class LocationData
    {
        [SerializeField] private ObjectIdentifier identifier;
        [SerializeField] private ObjectDescriptor descriptor;
        public ObjectIdentifier Identifier => identifier;
        public ObjectDescriptor Descriptor => descriptor;
    }
}