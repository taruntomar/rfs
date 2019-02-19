using RoomManagement;
using RoomManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace RFS_API.Controllers
{
    public class RoomsController : ApiController
    {
        IRoomManager _roomManager = null;
        public RoomsController(IRoomManager roomManager)
        {
            _roomManager = roomManager;
        }
        // GET api/<controller>
        public IEnumerable<Room> Get()
        {

            return _roomManager.GetAllRooms();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return _roomManager.GetRoomById(id);
        }

        // POST api/<controller>
        public void Post([FromBody]Room room)
        {
            _roomManager.AddNewRoom(room);
            
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]Room room)
        {
            _roomManager.UpdateRoomProperties(id, room);
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            _roomManager.DeleteRoom(id);
        }
    }
}
