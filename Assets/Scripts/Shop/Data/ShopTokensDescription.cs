using System;
using Test.Core.Data;
using UnityEngine;

namespace Test.Shop.Data
{
    [Serializable]
    public class ShopTokensDescription
    {
        [SerializeField] private ObjectIdentifier identifier;
        [SerializeField] private float amount;
        [SerializeField] private ShopItemType type;

        public ObjectIdentifier Identifier => identifier;
        public float Amount => amount;
        public ShopItemType Type => type;
    }
}