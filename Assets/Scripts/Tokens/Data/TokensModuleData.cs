using System.Collections.Generic;
using Test.Core.Data;
using UnityEngine;

namespace Test.Tokens.Data
{
    [CreateAssetMenu(menuName = "Modules/Data/Tokens/TokensModuleData")]
    public class TokensModuleData : ScriptableObject
    {
        [SerializeField] private List<TokenData> tokens;
        public List<TokenData> Tokens => tokens;

        private Dictionary<ObjectIdentifier, TokenDescriptor> mapping;

        public TokenDescriptor GetToken(ObjectIdentifier id)
        {
            return mapping.GetValueOrDefault(id);
        }

        private void OnEnable()
        {
            switch (mapping)
            {
                case null:
                    mapping = new Dictionary<ObjectIdentifier, TokenDescriptor>();
                    break;
                default:
                    mapping.Clear();
                    break;
            }

            foreach (var token in tokens) mapping[token.Identifier] = token.Descriptor;
        }
    }
}