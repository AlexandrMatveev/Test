using System.Collections.Generic;
using Test.Core.Data;
using UnityEngine;

namespace Test.Shop.Data
{
    [CreateAssetMenu(menuName = "Modules/Data/Shop/ShopModuleData")]
    public class ShopModuleData : ScriptableObject
    {
        [SerializeField] private ShopData shop;
        [SerializeField] private List<ObjectIdentifier> tracked;
        public ShopData Shop => shop;

        public List<ObjectIdentifier> Tracked => tracked;
    }
}