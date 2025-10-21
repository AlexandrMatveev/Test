using System;

namespace Test.Core.Communication
{
    /// <summary>
    /// Encapsulate all data about event subscriber
    /// </summary>
    public class Subscriber
    {
        public WeakReference TargetRef { get; }
        public Delegate Handler { get; }
        public Predicate<EventEnvelope> Filter { get; }

        public Subscriber(object target, Delegate handler, Predicate<EventEnvelope> filter)
        {
            TargetRef = new WeakReference(target);
            Handler = handler;
            Filter = filter;
        }
    }
}