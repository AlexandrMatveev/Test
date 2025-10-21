using System;
using System.Collections.Generic;
using Test.Core.Data;
using UnityEngine;

namespace Test.Location.Data
{
    [Serializable]
    public class LocationsData
    {
        [SerializeField] private ObjectIdentifier defaultLocation;
        [SerializeField] private List<LocationData> locations;
        public ObjectIdentifier DefaultLocation => defaultLocation;
        public List<LocationData> Locations => locations;
    }
}