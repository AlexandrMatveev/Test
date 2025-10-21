using System;
using System.Collections.Generic;
using System.Linq;
using Test.Core;
using Test.Core.Abstractions.Communication;
using Test.Core.Communication;
using Test.Core.Data;
using Test.Vip.Abstractions;
using Test.Vip.Controllers;
using Test.Vip.Data;
using UnityEngine;

namespace Test.Vip
{
    public class VipModule : BaseModule, IVipModule
    {
        private PartialPlayerData playerData;
        private VipModuleData moduleData;
        private IDisposable controller;
        private VipModel model;

        public override void InitializeModule()
        {
            moduleData = Container.Resolve<VipModuleData>();
            playerData = new PartialPlayerData
            {
                duration = moduleData.Vip.DefaultDuration
            };
            PlayerData.SetData<VipModule>(playerData);

            model = new VipModel(moduleData, playerData);
            model.OnModelChanged += PostModelUpdate;
        }

        public override void WorldChanged()
        {
            base.WorldChanged();
            BindControllerAndView();
        }

        private void BindControllerAndView()
        {
            controller?.Dispose();
            if (moduleData.Vip.Data.Descriptor.GameObject.Valid())
            {
                var view = moduleData.Vip.Data.Descriptor.GameObject.Resolve<GameObject>(Resolver);
                controller = new VipPlateController(model, view);
            }
        }

        protected override IEnumerable<IDisposable> Subscribe(EventBus bus)
        {
            var changeEvent = bus.Subscribe<ObjectChangeRequestEvent>(this, HandleChangeEvent, FilterEvent);
            var validationEvent = bus.Subscribe<ValidationEvent>(this, HandleValidationEvent, FilterEvent);
            return base.Subscribe(bus).Append(changeEvent).Append(validationEvent);
        }

        private void HandleChangeEvent(EventEnvelope env, ObjectChangeRequestEvent evt)
        {
            var ctx = (NumberEventContext)env.Source;
            switch (ctx.Type)
            {
                case NumberEventContext.Number.Integer:
                    model.AddAmountWithNotification(TimeSpan.FromSeconds(ctx.Value));
                    break;
                case NumberEventContext.Number.Percent:
                    model.AddPercentAmountWithNotification(ctx.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleValidationEvent(EventEnvelope env, ValidationEvent evt)
        {
            var ctx = (ValidationNumberEventContext)env.Source;
            switch (ctx.Type)
            {
                case NumberEventContext.Number.Integer:
                    if (model.CheckChargePossibility(TimeSpan.FromSeconds(ctx.Value))) ctx.SetValid();

                    break;
                case NumberEventContext.Number.Percent:
                    if (model.CheckChargePercentPossibility(ctx.Value)) ctx.SetValid();

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool FilterEvent(EventEnvelope env)
        {
            var context = env.Source as NumberEventContext;
            return (env.Event is ValidationEvent || env.Event is ObjectChangeRequestEvent)
                   && context is { Identifier: { } id }
                   && moduleData.Vip.Data.Identifier.Equals(id);
        }

        private void PostModelUpdate(ObjectIdentifier identifier)
        {
            Bus.Publish(new ObjectUpdateEvent(), new ObjectIdentifierEventContext(identifier));
        }
    }
}