using System;
using System.Collections.Generic;
using Test.Core.Data;
using UnityEngine;

namespace Test.Shop.Data
{
    [Serializable]
    public class ShopData
    {
        [SerializeField] private GameObjectBinding shopBinding;
        [SerializeField] private GameObjectBinding shopItemBinding;
        [SerializeField] private GameObjectBinding cardBinding;
        [SerializeField] private SceneIdentifier cardScene;
        [SerializeField] private SceneIdentifier shopScene;
        [SerializeField] private List<ShopItemData> items;

        public IReadOnlyList<ShopItemData> Items => items;
        public GameObjectBinding ShopBinding => shopBinding;
        public GameObjectBinding CardBinding => cardBinding;

        public GameObjectBinding ShopItemBinding => shopItemBinding;

        public SceneIdentifier CardScene => cardScene;

        public SceneIdentifier ShopScene => shopScene;
    }
}