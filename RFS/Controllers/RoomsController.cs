using RFS.Models.Entities;
using RoomManagement;
using RoomManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace RFS_API.Controllers
{
    public class RoomsController : ApiController
    {
        IRoomManager _roomManager = null;
        IBookingManager _bookingManager = null;
        ILocationManager _locationManager = null;
        public RoomsController(IRoomManager roomManager, IBookingManager bookingManager,ILocationManager locationManager)
        {
            _roomManager = roomManager;
            _bookingManager = bookingManager;
            _locationManager = locationManager;
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

        [System.Web.Http.Route("api/location/{locationId}/searchrooms")]
        [System.Web.Http.HttpGet()]
        public HttpResponseMessage GetsAvailableRoomsUnderLocation(HttpRequestMessage httpRequest, string locationId,string SdateTime, string EdateTime)
        {
            List<Room> availableRooms = new List<Room>();
            HttpResponseMessage response;
            DateTime s,e;
            if(!(DateTime.TryParse(SdateTime,out s) && DateTime.TryParse(EdateTime, out e)))
            {
                response = httpRequest.CreateResponse();
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                response.ReasonPhrase = "Unable to parse datetime";
                return response;
            }

            DateTime.TryParse(EdateTime, out e);
            
            var rooms = _roomManager.GetAllRoomsForLocation(locationId);
            foreach (var r in rooms) {
                if (_bookingManager.GetBookingForRoom(r.Id, s, e).Count() == 0)
                    availableRooms.Add(r);
            }
            response = httpRequest.CreateResponse(availableRooms);
            return response;
        }
        // GET api/<controller>/5
        public dynamic Get(string id)
        {
            System.Diagnostics.Trace.TraceInformation("getting room list");
            //System.Diagnostics.Trace.TraceWarning("This is a Warning");
            //System.Diagnostics.Trace.TraceError("This is an Error");
            var room = _roomManager.GetRoomById(id);
            var location = _locationManager.GetLocationById(room.location);

            return new { Id = room.Id,Location = new { Id= room.location, Name = location.Name, Country = location.Country }, MonitorScreen = room.MonitorScreen, Projector =  room.Projector, RoomName =  room.RoomName, Sitting =  room.Sitting, VideoConferencing_ = room.VideoConferencing_};
                
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
