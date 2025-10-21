using System;
using System.Linq;
using System.Threading.Tasks;
using Core.MVC;
using Test.Core.Abstractions.Communication;
using Test.Core.Communication;
using Test.Core.Data;
using Test.Shop.Data;

namespace Test.Shop.MVC
{
    public class ShopModel : Model<ShopModuleData, PartialPlayerData>
    {
        private EventBus bus;

        public ShopModel(ShopModuleData moduleData, PartialPlayerData playerData, EventBus bus) : base(moduleData, playerData)
        {
            this.bus = bus;
        }

        public async Task MakePurchase(ShopItemData item)
        {
            //Backend call simulation
            await Task.Delay(TimeSpan.FromSeconds(3));

            foreach (var price in item.Prices) MakeCharge(price);

            foreach (var reward in item.Rewards) MakeCharge(reward);
        }

        private void MakeCharge(ShopTokensDescription ds)
        {
            IEventContext ctx = ds.Type switch
            {
                ShopItemType.Number => new NumberEventContext(ds.Amount, NumberEventContext.Number.Integer, ds.Identifier),
                ShopItemType.Percent => new NumberEventContext(ds.Amount, NumberEventContext.Number.Percent, ds.Identifier),
                ShopItemType.Object => new ObjectIdentifierEventContext(ds.Identifier),
                _ => throw new ArgumentOutOfRangeException()
            };

            bus.Publish(new ObjectChangeRequestEvent(), ctx);
        }

        public bool CheckPurchasePossibility(ShopItemData item)
        {
            return item.Prices.All(CheckPurchasePossibility);
        }

        private bool CheckPurchasePossibility(ShopTokensDescription ds)
        {
            IValidationEventContext ctx = ds.Type switch
            {
                ShopItemType.Number => new ValidationNumberEventContext(ds.Amount, NumberEventContext.Number.Integer, ds.Identifier),
                ShopItemType.Percent => new ValidationNumberEventContext(ds.Amount, NumberEventContext.Number.Percent, ds.Identifier),
                ShopItemType.Object => new ValidationObjectIdentifierEventContext(ds.Identifier),
                _ => throw new ArgumentOutOfRangeException()
            };

            bus.Publish(new ValidationEvent(), ctx);

            return ctx is { Valid: true };
        }

        public ShopData GetShopData()
        {
            return ModuleData.Shop;
        }

        public void SetSelected(ShopItemData card)
        {
            PlayerData.selected = card.Data.Identifier;
        }

        public ShopItemData GetSelected()
        {
            return ModuleData.Shop.Items.First(i => i.Data.Identifier == PlayerData.selected);
        }

        public override ObjectIdentifier GetIdentifier()
        {
            return null;
        }

        public override ObjectDescriptor GetDescriptor()
        {
            return null;
        }
    }
}