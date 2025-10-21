using System;
using System.Collections.Generic;
using System.Linq;
using Test.Core;
using Test.Core.Abstractions.Communication;
using Test.Core.Communication;
using Test.Shop.Abstractions;
using Test.Shop.Data;
using Test.Shop.MVC;
using UnityEngine;

namespace Test.Shop
{
    public class ShopModule : BaseModule, IShopModule
    {
        private PartialPlayerData playerData;
        private ShopModuleData moduleData;
        private ShopModel model;
        private ShopController shopController;
        private CardController cardCotroller;

        public override void InitializeModule()
        {
            moduleData = Container.Resolve<ShopModuleData>();
            playerData = new PartialPlayerData();
            PlayerData.SetData<ShopModule>(playerData);

            model = new ShopModel(moduleData, playerData, Bus);
        }

        public override void WorldChanged()
        {
            base.WorldChanged();
            BindControllerAndView();
        }

        private void BindControllerAndView()
        {
            shopController?.Dispose();
            cardCotroller?.Dispose();

            if (moduleData.Shop.ShopBinding.Valid())
            {
                var view = moduleData.Shop.ShopBinding.Resolve<GameObject>(Resolver);
                shopController = new ShopController(model, view, Resolver);
            }

            if (moduleData.Shop.CardBinding.Valid())
            {
                var view = moduleData.Shop.CardBinding.Resolve<GameObject>(Resolver);
                cardCotroller = new CardController(model, view);
            }
        }

        protected override IEnumerable<IDisposable> Subscribe(EventBus bus)
        {
            return base.Subscribe(bus).Append(bus.Subscribe<ObjectUpdateEvent>(this, HandleEvent, FilterEvent));
        }

        private void HandleEvent(EventEnvelope env, ObjectUpdateEvent evt)
        {
            shopController?.ForceUpdateView();
            cardCotroller?.ForceUpdateView();
        }

        private bool FilterEvent(EventEnvelope env)
        {
            return env.Event is ObjectUpdateEvent
                   && env.Source is ObjectIdentifierEventContext { Identifier: { } id }
                   && moduleData.Tracked.Find(t => t.Equals(id));
        }
    }
}