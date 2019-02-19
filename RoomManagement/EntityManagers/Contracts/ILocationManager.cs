using System.Collections.Generic;
using RoomManagement.Entities;

namespace LocationManagement
{
    public interface ILocationManager
    {
        IList<Location> GetAllLocations();
        Location GetLocationById(string id);
        void AddNewLocation(Location roomName);
        void UpdateLocationProperties(string id, Location room);
        void DeleteLocation(string id);
    }
}