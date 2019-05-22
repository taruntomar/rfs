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

        public void DeleteRoom(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Room> GetAllRooms()
        {
            return _dbContext.Rooms.ToList<Room>();
        }

        public string GetRoomById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateRoomProperties(int id, Room room)
        {
            throw new NotImplementedException();
        }
    }
}
