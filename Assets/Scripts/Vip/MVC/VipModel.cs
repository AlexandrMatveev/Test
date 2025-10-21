using System;
using Core.MVC;
using Test.Core.Data;
using Test.Vip.Data;

namespace Test.Vip
{
    public class VipModel : Model<VipModuleData, PartialPlayerData>
    {
        public VipModel(VipModuleData moduleData, PartialPlayerData playerData) : base(moduleData, playerData)
        {
        }

        public TimeSpan GetCurrentDuration()
        {
            return PlayerData.duration;
        }

        public TimeSpan GetDefaultChargeDuration()
        {
            return ModuleData.Vip.DefaultChargeDuration;
        }

        public override ObjectIdentifier GetIdentifier()
        {
            return ModuleData.Vip.Data.Identifier;
        }

        public override ObjectDescriptor GetDescriptor()
        {
            return ModuleData.Vip.Data.Descriptor;
        }

        public TimeSpan AddAmount(TimeSpan value)
        {
            var newMS = Math.Clamp(PlayerData.duration.TotalMilliseconds + value.TotalMilliseconds, 0, int.MaxValue);
            var newDuration = TimeSpan.FromMilliseconds(newMS);
            return PlayerData.duration = newDuration;
        }

        public TimeSpan AddPercentAmount(float value)
        {
            var percent = PlayerData.duration + PlayerData.duration * value;
            var newMS = Math.Clamp(PlayerData.duration.TotalMilliseconds + percent.TotalMilliseconds, 0, int.MaxValue);
            var newDuration = TimeSpan.FromMilliseconds(newMS);
            return PlayerData.duration = newDuration;
        }

        public void AddAmountWithNotification(TimeSpan value)
        {
            AddAmount(value);
            NotifyModuleChange(GetIdentifier());
        }

        public void AddPercentAmountWithNotification(float value)
        {
            AddPercentAmount(value);
            NotifyModuleChange(GetIdentifier());
        }

        public bool CheckChargePossibility(TimeSpan value)
        {
            return PlayerData.duration.TotalMilliseconds + value.TotalMilliseconds >= 0;
        }

        public bool CheckChargePercentPossibility(float value)
        {
            var percent = PlayerData.duration + PlayerData.duration * value;
            return PlayerData.duration.TotalMilliseconds + percent.TotalMilliseconds >= 0;
        }
    }
}