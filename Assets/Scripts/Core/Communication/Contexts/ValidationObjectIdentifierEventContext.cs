using Test.Core.Abstractions.Communication;
using Test.Core.Data;

namespace Test.Core.Communication
{
    public class ValidationObjectIdentifierEventContext : ObjectIdentifierEventContext, IValidationEventContext
    {
        public bool Valid { get; private set; }

        public ValidationObjectIdentifierEventContext(ObjectIdentifier identifier) : base(identifier)
        {
        }

        public void SetValid()
        {
            Valid = true;
        }
    }
}