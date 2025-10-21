using System;
using System.Collections.Generic;
using System.Linq;
using Test.Core;
using Test.Core.Abstractions.Communication;
using Test.Core.Communication;
using Test.Location.Controllers;
using Test.Location.Data;
using Test.Location.Abstractions;
using UnityEngine;

namespace Test.Location
{
    public class LocationsModule : BaseModule, ILocationsModule
    {
        private PartialPlayerData partialPlayerData;
        private LocationsModuleData moduleData;
        private IDisposable controller;
        private LocationModel model;

        public override void InitializeModule()
        {
            moduleData = Container.Resolve<LocationsModuleData>();
            partialPlayerData = new PartialPlayerData
            {
                location = moduleData.Locations.DefaultLocation
            };
            PlayerData.SetData<LocationsModule>(partialPlayerData);

            model = new LocationModel(moduleData, partialPlayerData);
        }

        public override void WorldChanged()
        {
            base.WorldChanged();
            BindControllerAndView();
        }

        protected override IEnumerable<IDisposable> Subscribe(EventBus bus)
        {
            return base.Subscribe(bus).Append(bus.Subscribe<ObjectChangeRequestEvent>(this, HandleEvent, FilterEvent));
        }

        private void HandleEvent(EventEnvelope env, ObjectChangeRequestEvent evt)
        {
            var context = (ObjectIdentifierEventContext)env.Source;
            model.UpdateLocationWithNotification(context.Identifier);
        }

        private bool FilterEvent(EventEnvelope env)
        {
            return env.Source is ObjectIdentifierEventContext { Identifier: { } id }
                   && moduleData.GetLocation(id) != null;
        }

        private void BindControllerAndView()
        {
            controller?.Dispose();
            var description = moduleData.GetLocation(partialPlayerData.location);
            if (description.GameObject.Valid())
            {
                var view = description.GameObject.Resolve<GameObject>(Resolver);
                controller = new LocationPlateController(model, view);
            }
        }
    }
}