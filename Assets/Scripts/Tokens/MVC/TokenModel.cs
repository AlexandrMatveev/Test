using System;
using Core.MVC;
using Test.Core.Data;
using Test.Tokens.Data;

namespace Test.Tokens.MVC
{
    public class TokenModel : Model<TokensModuleData, PartialPlayerData>
    {
        private ObjectIdentifier identifier;
        private TokenDescriptor description;
        private PartialPlayerDataTokenData tokenData;

        public TokenModel(ObjectIdentifier identifier, TokensModuleData moduleData, PartialPlayerData playerData) : base(moduleData, playerData)
        {
            this.identifier = identifier;
            description = moduleData.GetToken(identifier);
            tokenData = playerData.GetToken(identifier);
        }

        public override ObjectIdentifier GetIdentifier()
        {
            return identifier;
        }

        public override ObjectDescriptor GetDescriptor()
        {
            return description.Descriptor;
        }

        public int GetAmount()
        {
            return tokenData.amount;
        }

        public int GetChargeAmount()
        {
            return description.DefaultChargeAmount;
        }

        public int AddAmount(float value)
        {
            return tokenData.amount = (int)Math.Clamp(tokenData.amount + value, 0, int.MaxValue);
        }

        public int AddPercentAmount(float value)
        {
            var percent = tokenData.amount * value;
            return tokenData.amount = (int)Math.Clamp(tokenData.amount + percent, 0, int.MaxValue);
        }

        public void AddAmountWithNotification(float value)
        {
            AddAmount(value);
            NotifyModuleChange(GetIdentifier());
        }

        public void AddPercentAmountWithNotification(float value)
        {
            AddPercentAmount(value);
            NotifyModuleChange(GetIdentifier());
        }

        public bool CheckChargePossibility(float value)
        {
            return tokenData.amount + value >= 0;
        }

        public bool CheckChargePercentPossibility(float value)
        {
            var percent = tokenData.amount * value;
            return tokenData.amount + percent >= 0;
        }
    }
}