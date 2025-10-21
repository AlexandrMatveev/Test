using Test.Core.Abstractions.Communication;
using Test.Core.Data;

namespace Test.Core.Communication
{
    /// <summary>
    /// Validation contextual object contains number format and additional validation flag
    /// </summary>
    public class ValidationNumberEventContext : NumberEventContext, IValidationEventContext
    {
        public bool Valid { get; private set; }

        public ValidationNumberEventContext(float number, Number type, ObjectIdentifier identifier = null) : base(number, type, identifier)
        {
        }

        public void SetValid()
        {
            Valid = true;
        }
    }
}