using System.Collections.Generic;
using Test.Core.Data;
using UnityEngine;

namespace Test.Location.Data
{
    [CreateAssetMenu(menuName = "Modules/Data/Locations/LocationsModuleData")]
    public class LocationsModuleData : ScriptableObject
    {
        [SerializeField] private LocationsData locations;
        public LocationsData Locations => locations;

        private Dictionary<ObjectIdentifier, ObjectDescriptor> mapping;

        public ObjectDescriptor GetLocation(ObjectIdentifier id)
        {
            return mapping.GetValueOrDefault(id);
        }

        private void OnEnable()
        {
            switch (mapping)
            {
                case null:
                    mapping = new Dictionary<ObjectIdentifier, ObjectDescriptor>();
                    break;
                default:
                    mapping.Clear();
                    break;
            }

            foreach (var location in locations.Locations) mapping[location.Identifier] = location.Descriptor;
        }
    }
}