using System;
using System.Collections.Generic;
using Test.Core.Data;
using UnityEngine;

namespace Test.Shop.Data
{
    [Serializable]
    public class ShopItemData
    {
        [SerializeField] private ObjectData data;
        [SerializeField] private List<ShopTokensDescription> prices;
        [SerializeField] private List<ShopTokensDescription> rewards;

        public ObjectData Data => data;
        public IEnumerable<ShopTokensDescription> Prices => prices;

        public List<ShopTokensDescription> Rewards => rewards;
    }
}