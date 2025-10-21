using Test.Core.Abstractions.Communication;
using Test.Core.Data;

namespace Test.Core.Communication
{
    /// <summary>
    /// Contextual object represent some object identifier
    /// </summary>
    public class ObjectIdentifierEventContext : IEventContext
    {
        public ObjectIdentifier Identifier { get; }

        public ObjectIdentifierEventContext(ObjectIdentifier identifier)
        {
            Identifier = identifier;
        }
    }
}