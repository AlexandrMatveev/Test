using Test.Core.Abstractions.Communication;
using Test.Core.Data;

namespace Test.Core.Communication
{
    /// <summary>
    /// Contextual object represent specific number and number format
    /// </summary>
    public class NumberEventContext : IEventContext
    {
        public enum Number : byte
        {
            Integer,
            Percent
        }

        public ObjectIdentifier Identifier { get; }

        public float Value { get; }
        public Number Type { get; }

        public NumberEventContext(float number, Number type, ObjectIdentifier identifier = null)
        {
            Identifier = identifier;
            Value = number;
            Type = type;
        }
    }
}