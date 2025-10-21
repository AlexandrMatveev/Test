namespace Test.Core.Abstractions.Communication
{
    public interface IValidationEventContext : IEventContext
    {
        bool Valid { get; }
    }
}