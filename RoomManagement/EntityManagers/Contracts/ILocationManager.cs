using System.Collections.Generic;
using RoomManagement.Entities;

namespace RoomManagement
{
    public interface ILocationManager
    {
        IList<Location> GetAllLocations();
        Location GetLocationById(string id);
        string AddNewLocation(Location roomName);
        void UpdateLocationProperties(string id, Location room);
        void DeleteLocation(string id);
    }
}