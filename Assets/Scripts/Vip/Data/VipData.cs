using System;
using Test.Core.Data;
using UnityEngine;

namespace Vip.Data
{
    [Serializable]
    public class VipData
    {
        [SerializeField] private ObjectData data;
        [SerializeField] private int defaultDuration;
        [SerializeField] private int defaultChargeDuration;

        public ObjectData Data => data;
        public TimeSpan DefaultDuration => TimeSpan.FromSeconds(defaultDuration);
        public TimeSpan DefaultChargeDuration => TimeSpan.FromSeconds(defaultChargeDuration);
    }
}