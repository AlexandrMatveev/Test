using Test.Core.Abstractions.Communication;

namespace Test.Core.Communication
{
    /// <summary>
    /// Encapsulate of possible information about event and event's context
    /// </summary>
    public class EventEnvelope
    {
        public IEvent Event { get; }
        public IEventContext Source { get; }

        public EventEnvelope(IEvent evt, IEventContext src = null)
        {
            Event = evt;
            Source = src;
        }
    }
}