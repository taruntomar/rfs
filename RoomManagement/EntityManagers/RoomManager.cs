using RoomManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomManagement
{
    public class RoomManager : IRoomManager
    {
        private IRoomManagementDatabasseEntities _dbContext;
        public RoomManager(IRoomManagementDatabasseEntities dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddNewRoom(Room room)
        {
            _dbContext.Rooms.Add(room);
            _dbContext.SaveChanges();
        }

        public void DeleteRoom(string id)
        {
            Room room =  GetRoomById(id);
            if (room != null)
            {
                _dbContext.Rooms.Remove(room);
                _dbContext.SaveChanges();
            }
        }

        public IList<Room> GetAllRooms()
        {
            return _dbContext.Rooms.ToList<Room>();
        }

        public Room GetRoomById(string id)
        {
          return  _dbContext.Rooms.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateRoomProperties(string id, Room newroom)
        {
            Room room = GetRoomById(id);
            if (room != null)
            {
                room.location = newroom.location;
                room.MonitorScreen = newroom.MonitorScreen;
                room.Projector = newroom.Projector;
                room.RoomName = newroom.RoomName;
                room.Sitting = newroom.Sitting;
                room.VideoConferencing_ = newroom.VideoConferencing_;
                _dbContext.SaveChanges();
            }
        }
    }
}
