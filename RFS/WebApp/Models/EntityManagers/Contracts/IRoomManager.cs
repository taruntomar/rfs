using System.Collections.Generic;
using RFS.Models.Entities;
using RoomManagement.Entities;

namespace RoomManagement
{
    public interface IRoomManager
    {
        void AddNewRoom(Room room);
        void DeleteRoom(string id);
        IList<Room> GetAllRooms();
        Room GetRoomById(string id);
        void UpdateRoomProperties(string id, Room newroom);
        IEnumerable<Room> GetAllRoomsForLocation(string locationId);
    }
}