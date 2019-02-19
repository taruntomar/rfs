using System.Collections.Generic;
using RoomManagement.Entities;

namespace RoomManagement
{
    public interface IRoomManager
    {
        IList<Room> GetAllRooms();
        string GetRoomById(int id);
        void AddNewRoom(string roomName);
        void UpdateRoomProperties(int id, Room room);
        void DeleteRoom(int id);
    }
}