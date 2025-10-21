using System;
using System.Collections.Generic;
using System.Linq;
using Test.Core;
using Test.Core.Abstractions.Communication;
using Test.Core.Communication;
using Test.Core.Data;
using Test.Tokens.Abstractions;
using Test.Tokens.Data;
using Test.Tokens.MVC;
using UnityEngine;

namespace Test.Tokens
{
    public class TokensModule : BaseModule, ITokensModule
    {
        private PartialPlayerData playerData;
        private TokensModuleData moduleData;

        private HashSet<IDisposable> controllers;
        private Dictionary<ObjectIdentifier, TokenModel> models;

        public override void InitializeModule()
        {
            moduleData = Container.Resolve<TokensModuleData>();
            playerData = new PartialPlayerData
            {
                tokens = moduleData.Tokens.Select(t => new PartialPlayerDataTokenData { id = t.Identifier, amount = t.Descriptor.DefaultAmount }).ToList()
            };
            PlayerData.SetData<TokensModule>(playerData);

            models = new Dictionary<ObjectIdentifier, TokenModel>();
            controllers = new HashSet<IDisposable>(models.Count);
            foreach (var token in moduleData.Tokens)
            {
                var model = new TokenModel(token.Identifier, moduleData, playerData);
                model.OnModelChanged += PostModelUpdate;
                models.Add(token.Identifier, model);
            }
        }

        public override void WorldChanged()
        {
            base.WorldChanged();
            BindControllerAndView();
        }

        private void BindControllerAndView()
        {
            foreach (var controller in controllers) controller.Dispose();

            controllers.Clear();

            foreach (var token in moduleData.Tokens)
                if (token.Descriptor.Descriptor.GameObject.Valid())
                {
                    var counterView = token.Descriptor.Descriptor.GameObject.Resolve<GameObject>(Resolver);
                    var counterCounterPlateController = new TokenCounterPlateController(models[token.Identifier], counterView);
                    controllers.Add(counterCounterPlateController);
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
            var id = ctx.Identifier;
            switch (ctx.Type)
            {
                case NumberEventContext.Number.Integer:
                    models[id].AddAmountWithNotification((int)ctx.Value);
                    break;
                case NumberEventContext.Number.Percent:
                    models[id].AddPercentAmountWithNotification(ctx.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleValidationEvent(EventEnvelope env, ValidationEvent evt)
        {
            var ctx = (ValidationNumberEventContext)env.Source;
            var id = ctx.Identifier;
            switch (ctx.Type)
            {
                case NumberEventContext.Number.Integer:
                    if (models[id].CheckChargePossibility((int)ctx.Value)) ctx.SetValid();

                    break;
                case NumberEventContext.Number.Percent:
                    if (models[id].CheckChargePercentPossibility(ctx.Value)) ctx.SetValid();

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
                   && models.ContainsKey(id);
        }

        private void PostModelUpdate(ObjectIdentifier identifier)
        {
            Bus.Publish(new ObjectUpdateEvent(), new ObjectIdentifierEventContext(identifier));
        }
    }
}