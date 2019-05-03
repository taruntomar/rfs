using RFS.Models;
using RFS.Models.Entities;
using RoomManagement;
using RoomManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace RFS_API.Controllers
{
    public class BookingController : ApiController
    {
        IBookingManager _bookingManager = null;
        public BookingController(IBookingManager bookingManager)
        {
            _bookingManager = bookingManager;
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
        public void Post([FromBody]Booking booking)
        {
            TApiAuth auth = new TApiAuth();
            booking.Id = Guid.NewGuid().ToString();
            booking.createdBy = auth.GetLoggedInUsername(Request);
            booking.createdOn = DateTime.UtcNow;
            _bookingManager.AddNewBooking(booking);
        }

        // PUT api/<controller>/5
        public void Put(string id, [FromBody]Booking booking)
        {
            if (IsAuth())
                _bookingManager.UpdateBooking(id, booking);
            throw new HttpResponseException(HttpStatusCode.Unauthorized);

        }

        // DELETE api/<controller>/5
        public void Delete(string id)
        {
            if (IsAuth())
                _bookingManager.DeleteBooking(id);
            throw new HttpResponseException(HttpStatusCode.Unauthorized);

        }
    }
}
