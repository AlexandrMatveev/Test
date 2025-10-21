using UnityEngine;
using Vip.Data;

namespace Test.Vip.Data
{
    [CreateAssetMenu(menuName = "Modules/Data/Vip/VipModuleData")]
    public class VipModuleData : ScriptableObject
    {
        [SerializeField] private VipData vip;
        public VipData Vip => vip;
    }
}