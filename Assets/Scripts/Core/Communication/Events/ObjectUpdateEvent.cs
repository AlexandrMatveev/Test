namespace Test.Core.Abstractions.Communication
{
    /// <summary>
    /// Shows that that object state was changed,
    /// usefully to show for specific modules that what notify others about state change
    /// </summary>
    public struct ObjectUpdateEvent : IEvent
    {
    }
}