using System;
using System.Collections.Generic;
using Test.Core.Data;

namespace Test.Tokens.Data
{
    [Serializable]
    public class PartialPlayerData
    {
        public List<PartialPlayerDataTokenData> tokens = new();

        public PartialPlayerDataTokenData GetToken(ObjectIdentifier id)
        {
            return tokens.Find(d => d.id.Equals(id));
        }
    }
}