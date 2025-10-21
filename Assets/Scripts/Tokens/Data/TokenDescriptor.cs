using System;
using Test.Core.Data;
using UnityEngine;

namespace Test.Tokens.Data
{
    [Serializable]
    public class TokenDescriptor
    {
        [SerializeField] private ObjectDescriptor descriptor;
        [SerializeField] private int defaultAmount;
        [SerializeField] private int defaultChargeAmount;

        public ObjectDescriptor Descriptor => descriptor;
        public int DefaultAmount => defaultAmount;
        public int DefaultChargeAmount => defaultChargeAmount;
    }
}