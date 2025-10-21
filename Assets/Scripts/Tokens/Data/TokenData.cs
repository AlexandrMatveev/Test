using System;
using Test.Core.Data;
using UnityEngine;

namespace Test.Tokens.Data
{
    [Serializable]
    public class TokenData
    {
        [SerializeField] private ObjectIdentifier identifier;
        [SerializeField] private TokenDescriptor descriptor;
        public ObjectIdentifier Identifier => identifier;
        public TokenDescriptor Descriptor => descriptor;
    }
}