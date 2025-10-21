using Core.MVC;
using Test.Core.Data;
using Test.Location.Data;

namespace Test.Location
{
    public class LocationModel : Model<LocationsModuleData, PartialPlayerData>
    {
        public LocationModel(LocationsModuleData moduleData, PartialPlayerData playerData) : base(moduleData, playerData)
        {
        }

        public override ObjectIdentifier GetIdentifier()
        {
            return PlayerData.location;
        }

        public override ObjectDescriptor GetDescriptor()
        {
            return ModuleData.GetLocation(PlayerData.location);
        }

        public ObjectIdentifier GetDefaultIdentifier()
        {
            return ModuleData.Locations.DefaultLocation;
        }

        public ObjectDescriptor GetDefaultDescriptor()
        {
            return ModuleData.GetLocation(ModuleData.Locations.DefaultLocation);
        }

        public ObjectDescriptor UpdateLocation(ObjectIdentifier newLocation)
        {
            PlayerData.location = newLocation;
            return ModuleData.GetLocation(newLocation);
        }

        public void UpdateLocationWithNotification(ObjectIdentifier newLocation)
        {
            UpdateLocation(newLocation);
            NotifyModuleChange(GetIdentifier());
        }
    }
}