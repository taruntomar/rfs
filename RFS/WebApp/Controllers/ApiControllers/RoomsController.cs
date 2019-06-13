using RFS.Models;
using RFS.Models.Entities;
using RoomManagement;
using RoomManagement.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        IUserManager _userManager = null;

        public RoomsController(IUserManager userManager,IRoomManager roomManager, IBookingManager bookingManager,ILocationManager locationManager)
        {
            _roomManager = roomManager;
            _bookingManager = bookingManager;
            _locationManager = locationManager;
            _userManager = userManager;
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

            return _roomManager.GetAllRoomsForLocation(locationId).Select(x=> new Room() {Id=x.Id,decommission = x.decommission, location= x.location, MonitorScreen = x.MonitorScreen,  Projector = x.Projector, RoomName = x.RoomName,  Sitting = x.Sitting, VideoConferencing_ = x.VideoConferencing_ });
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/rooms/{roomid}/dp")]
        public async Task<IHttpActionResult> SetDP(string roomid)
        {
            var useremail = new TApiAuth().GetLoggedInUsername(Request);
            if (string.IsNullOrEmpty(useremail))
            {
                return BadRequest();
            }
            var user = _userManager.GetUserFromMailId(useremail);
            if (user.isAdmin.HasValue && user.isAdmin.Value)
            {
                if (!Request.Content.IsMimeMultipartContent())
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var file in provider.Contents)
                {
                    var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                    var buffer = await file.ReadAsByteArrayAsync();
                    _roomManager.SetRoomProfilePic(roomid, buffer, filename);
                }

                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/rooms/{roomid}/dp")]
        public HttpResponseMessage GetDP(string roomid)
        {
            var useremail = new TApiAuth().GetLoggedInUsername(Request);
            if (string.IsNullOrEmpty(useremail))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            var room = _roomManager.GetRoomById(roomid);
            if (room == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            var profilepic = room.RoomProfilePics.FirstOrDefault();
            Byte[] b;
            if (profilepic == null)
            {
                string path = HttpContext.Current.Server.MapPath("~\\Content\\img\\room-outline.png");
                int a = 1;
                b = File.ReadAllBytes(path);
                //using (FileStream fs = new FileStream(path, FileMode.Open))
                //{
                //    response.Content = new StreamContent(fs);
                //}

            }
            else
            {
                b = profilepic.data;


            }
            response.Content = new StreamContent(new MemoryStream(b));
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            return response;
        }
        [System.Web.Http.Route("api/location/{locationId}/searchrooms")]
        [System.Web.Http.HttpGet()]
        public HttpResponseMessage GetsAvailableRoomsUnderLocation(HttpRequestMessage httpRequest, string locationId,string SdateTime, string EdateTime)
        {
            List<object> availableRooms = new List<object>();
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
            foreach (var r in rooms)
            {
                if (r.decommission.HasValue && r.decommission.Value)
                    continue;
                if (_bookingManager.GetBookingForRoom(r.Id, s, e).Count() == 0)
                {
                    var room = new { Id = r.Id, location = r.location, MonitorScreen = r.MonitorScreen, Projector = r.Projector, RoomName= r.RoomName, Sitting=r.Sitting, VideoConferencing=r.VideoConferencing_,image = "Content\\img\\room.jpg" };
                    availableRooms.Add(room);
                }
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
        public HttpResponseMessage Post([FromBody]Room room)
        {
            var useremail = new TApiAuth().GetLoggedInUsername(Request);
            if (string.IsNullOrEmpty(useremail))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            var user = _userManager.GetUserFromMailId(useremail);
            if (user.isAdmin.HasValue && user.isAdmin.Value)
            {
                room.Id = Guid.NewGuid().ToString();
                _roomManager.AddNewRoom(room);
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }

        // PUT api/<controller>/5
        public IHttpActionResult Put(string id, [FromBody]Room room)
        {
            var useremail = new TApiAuth().GetLoggedInUsername(Request);

            if (!string.IsNullOrEmpty(useremail))
            {

                user u = _userManager.GetUserFromMailId(useremail);
                if (u != null && u.isAdmin.HasValue && u.isAdmin.Value)
                    _roomManager.UpdateRoomProperties(id, room);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        // DELETE api/<controller>/5
        public IHttpActionResult Delete(string id)
        {
            var useremail = new TApiAuth().GetLoggedInUsername(Request);
            
            if (!string.IsNullOrEmpty(useremail)){

                user u = _userManager.GetUserFromMailId(useremail);
                if(u!=null && u.isAdmin.HasValue && u.isAdmin.Value)
                _roomManager.DeleteRoom(id);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }

        }
    }
}
