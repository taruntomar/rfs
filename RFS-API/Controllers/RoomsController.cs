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
        IBookingManager _bookingManager = null;
        public RoomsController(IRoomManager roomManager, IBookingManager bookingManager)
        {
            _roomManager = roomManager;
            _bookingManager = bookingManager;
        }
        // GET api/<controller>
        public IEnumerable<Room> Get()
        {

            return _roomManager.GetAllRooms();
        }

        [System.Web.Http.Route("api/location/{locationId}/rooms")]
        [System.Web.Http.HttpGet()]
        public IEnumerable<Room> GetRoomsUnderLocation(string locationId)
        {

            return _roomManager.GetAllRoomsForLocation(locationId);
        }

        [System.Web.Http.Route("api/location/{locationId}/rooms/{SdateTime:DateTime}/{EdateTime:DateTime}")]
        [System.Web.Http.HttpGet()]
        public IEnumerable<Room> GetsAvailableRoomsUnderLocation(string locationId,DateTime SdateTime, DateTime EdateTime)
        {
            List<Room> availableRooms = new List<Room>();
            var rooms = _roomManager.GetAllRoomsForLocation(locationId);
            foreach (var r in rooms) {
                if (_bookingManager.GetBookingForRoom(r.Id, SdateTime, EdateTime).Count() == 0)
                    availableRooms.Add(r);
            }
            return availableRooms;
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
