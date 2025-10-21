using System;
using System.Collections.Generic;
using System.Linq;
using Test.Core.Abstractions.Communication;

namespace Test.Core.Communication
{
    /// <summary>
    /// Simple implementation of event bus pattern
    /// </summary>
    public class EventBus
    {
        private Dictionary<Type, List<Subscriber>> subscribers = new();
        private Dictionary<Type, Type[]> typeClosureCache = new();

        public Subscription<T> Subscribe<T>(object target, Action<EventEnvelope, T> handler, Predicate<EventEnvelope> filter = null) where T : IEvent
        {
            var subscriber = new Subscriber(target, handler, filter);
            if (!subscribers.TryGetValue(typeof(T), out var list)) list = subscribers[typeof(T)] = new List<Subscriber>();

            list.Add(subscriber);
            return new Subscription<T>(this, subscriber);
        }

        public void Unsubscribe<T>(Subscriber subscriber) where T : IEvent
        {
            if (subscribers.TryGetValue(typeof(T), out var list)) list.Remove(subscriber);
        }

        public void Publish<T>(T evt, IEventContext source = null) where T : IEvent
        {
            var env = new EventEnvelope(evt, source);
            Deliver(env);
        }

        private void Deliver(EventEnvelope env)
        {
            var types = GetTypeClosure(env.Event.GetType());
            foreach (var type in types)
                if (subscribers.TryGetValue(type, out var list))
                {
                    var snapshot = list.ToArray();
                    foreach (var subscriber in snapshot)
                    {
                        if (!subscriber.TargetRef.IsAlive)
                        {
                            list.Remove(subscriber);
                            continue;
                        }

                        if (subscriber.Filter != null && !subscriber.Filter(env)) continue;

                        switch (subscriber.Handler)
                        {
                            case Action<EventEnvelope, object> aObj:
                                aObj(env, env.Event);
                                break;
                            case Action<EventEnvelope, IEvent> aAny:
                                aAny(env, env.Event);
                                break;
                            default:
                            {
                                var d = subscriber.Handler;
                                var parms = d.Method.GetParameters();
                                if (parms.Length == 2 && parms[1].ParameterType.IsAssignableFrom(type))
                                {
                                    d.DynamicInvoke(env, env.Event);
                                }
                                else
                                {
                                    if (d is not null) d.DynamicInvoke(env, env.Event);
                                }

                                break;
                            }
                        }
                    }

                    break;
                }
        }

        private Type[] GetTypeClosure(Type eventType)
        {
            if (typeClosureCache.TryGetValue(eventType, out var result)) return result;

            var set = new HashSet<Type>();

            for (var c = eventType; c != null; c = c.BaseType) set.Add(c);

            foreach (var it in eventType.GetInterfaces()) set.Add(it);

            if (typeof(IEvent).IsAssignableFrom(eventType)) set.Add(typeof(IEvent));

            return typeClosureCache[eventType] = set.ToArray();
        }
    }
}