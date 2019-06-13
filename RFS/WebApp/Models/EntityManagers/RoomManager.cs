using RFS.Models.Entities;
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
            //cancel all bookings related to this room
            Room room = GetRoomById(id);
            if (room != null)
            {
                var bookings = _dbContext.Bookings.Where(x => x.RoomId == id && x.starttime.CompareTo(DateTime.UtcNow)>0);            
                foreach(var booking in bookings)
                {
                    booking.isCancelled = true;
                }
                room.decommission = true;
            
                //_dbContext.Rooms.Remove(room);
                _dbContext.SaveChanges();
            }
        }

        public IList<Room> GetAllRooms()
        {
            return _dbContext.Rooms.ToList<Room>();
        }

        public IEnumerable<Room> GetAllRoomsForLocation(string locationId)
        {
            return _dbContext.Rooms.Where(x => x.location == locationId);
        }

        public Room GetRoomById(string id)
        {
          return  _dbContext.Rooms.FirstOrDefault(x => x.Id == id);
        }

        public void SetRoomProfilePic(string roomId, byte[] buffer, string filename)
        {
            var room = _dbContext.Rooms.FirstOrDefault(x=>x.Id==roomId);
            RoomProfilePic roomProfilePic = null;
            if (room.RoomProfilePics == null || room.RoomProfilePics.Count == 0)
            {
                roomProfilePic = new RoomProfilePic();
                roomProfilePic.Id = Guid.NewGuid().ToString();
                _dbContext.RoomProfilePicture.Add(roomProfilePic);
            }
            else
            {
                roomProfilePic = room.RoomProfilePics.FirstOrDefault();
            }

            roomProfilePic.data = buffer;
            roomProfilePic.ext = filename.Split('.')[1];
            roomProfilePic.RoomId = room.Id;
            _dbContext.SaveChanges();
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
