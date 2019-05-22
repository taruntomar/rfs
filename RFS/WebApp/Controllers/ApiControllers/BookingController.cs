using RFS.Models;
using RFS.Models.Entities;
using RoomManagement;
using RoomManagement.Entities;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace RFS_API.Controllers
{
    public class BookingController : ApiController
    {
        IBookingManager _bookingManager = null;
        IRoomManager _roomManager = null;
        IUserManager _userManager = null;
        ILocationManager _locationManager = null;
        public BookingController(IBookingManager bookingManager,IRoomManager roomManager, IUserManager userManager,ILocationManager locationManager)
        {
            _bookingManager = bookingManager;
            _roomManager = roomManager;
            _userManager = userManager;
            _locationManager = locationManager;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/me/bookings/{timeframe}")]
        public dynamic GetBookings(string timeframe)
        {
            var username = new TApiAuth().GetLoggedInUsername(Request);
            if(string.IsNullOrEmpty(username))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

           
            var bookings = _bookingManager.GetBookingDoneByUser(username);
            var compare = timeframe == "upcoming" ? 1 : -1;
            var filter = bookings.Where(x => x.starttime.CompareTo(DateTime.UtcNow) == compare);
            var result = filter.Select(x=> {return new { Id = x.Id, isCancelled = x.isCancelled, Room = new { Id = x.RoomId, image = "Content\\img\\room.jpg", Name = _roomManager.GetRoomById(x.RoomId).RoomName }, Date = x.starttime.ToShortDateString(), StartTime = x.starttime.ToShortTimeString(), EndTime = x.endtime.ToShortTimeString(), BookedOn = x.createdOn }; });
            return result;
        }
        // GET api/<controller>
        public IEnumerable<Booking> Get(string roomId, DateTime startDateTime, DateTime endDateTime)
        {
            if(IsAuth())
            return _bookingManager.GetBookingForRoom(roomId, startDateTime, endDateTime);

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        private bool IsAuth()
        {
           return string.IsNullOrEmpty(new TApiAuth().GetLoggedInUsername(Request)) ? false : true;
        }

        // GET api/<controller>/5
        public Booking Get(string id)
        {
            if (IsAuth())
                return _bookingManager.GetBookingById(id);
            throw new HttpResponseException(HttpStatusCode.Unauthorized);

        }


        // POST api/<controller>
        public async Task PostAsync([FromBody]Booking booking)
        {
            try
            {
                TApiAuth auth = new TApiAuth();
                booking.Id = Guid.NewGuid().ToString();
                booking.createdBy = auth.GetLoggedInUsername(Request);
                booking.createdOn = DateTime.UtcNow;
                _bookingManager.AddNewBooking(booking);
                var room = _roomManager.GetRoomById(booking.RoomId);
                var user = _userManager.GetUserFromMailId(booking.createdBy);
                var loc = _locationManager.GetLocationById(room.location);
                //await SendEmailExecute(booking, room, user,"done");
                var host = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host + ":" + Request.RequestUri.Port;
                EmailComManager emailComManager = new EmailComManager(host);

                await emailComManager.SendRoomBookingCalenderInvite(user.email, user.Name, room.RoomName + "(" + loc.Name + ")", booking.starttime, booking.endtime);
            }catch(Exception ex)
            {

            }
        }
        async Task SendEmailExecute(Booking booking, Room room, user user, string message)
        {
            var apiKey = System.Configuration.ConfigurationManager.AppSettings["sendgridkey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("india-facility@resideo.com", "Meeting Room Booking");
            var subject = "Booking is "+ message + " for room \""+ room.RoomName+ "\"";
            var to = new EmailAddress(user.email, user.Name);
            var plainTextContent = "Room: "+room.RoomName+", "+booking.starttime+"-"+booking.endtime;
            var htmlContent = "<a>Check on Facility Portal</a>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        // PUT api/<controller>/5
        public void Put(string id, [FromBody]Booking booking)
        {
            if (IsAuth())
                _bookingManager.UpdateBooking(id, booking);
            throw new HttpResponseException(HttpStatusCode.Unauthorized);

        }

        // DELETE api/<controller>/5
        public async Task DeleteAsync(string id)
        {
            if (IsAuth())
            {
                _bookingManager.DeleteBooking(id);
                var booking = _bookingManager.GetBookingById(id);
                var room = _roomManager.GetRoomById(booking.RoomId);
                var user = _userManager.GetUserFromMailId(booking.createdBy);
                await SendEmailExecute(booking, room, user, "cancelled");

            }
            else
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

        }
    }
}
