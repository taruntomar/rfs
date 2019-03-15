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

        [System.Web.Http.Route("location/{locationId}/rooms")]
        [System.Web.Http.HttpGet()]
        public IEnumerable<Room> GetRoomsUnderLocation(string locationId)
        {

            return _roomManager.GetAllRoomsForLocation(locationId);
        }
        // GET api/<controller>/5
        public Room Get(string id)
        {
            System.Diagnostics.Trace.TraceInformation("getting room list");
            //System.Diagnostics.Trace.TraceWarning("This is a Warning");
            //System.Diagnostics.Trace.TraceError("This is an Error");

            return _roomManager.GetRoomById(id);
        }

        // POST api/<controller>
        public void Post([FromBody]Room room)
        {
            _roomManager.AddNewRoom(room);
            
        }

        // PUT api/<controller>/5
        public void Put(string id, [FromBody]Room room)
        {
            _roomManager.UpdateRoomProperties(id, room);
        }

        // DELETE api/<controller>/5
        public void Delete(string id)
        {
            _roomManager.DeleteRoom(id);
        }
    }
}
