using System;
using Test.Core.Abstractions.Communication;

namespace Test.Core.Communication
{
    /// <summary>
    /// Client side subscription handle
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Subscription<T> : IDisposable where T : IEvent
    {
        private readonly EventBus bus;
        private readonly Subscriber subscriber;
        private bool disposed;

        internal Subscription(EventBus bus, Subscriber subscriber)
        {
            this.bus = bus;
            this.subscriber = subscriber;
        }

        public void Dispose()
        {
            if (disposed) return;

            disposed = true;
            bus.Unsubscribe<T>(subscriber);
        }
    }
}