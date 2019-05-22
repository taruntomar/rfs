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
            return _bookingManager.GetBookingForRoom(roomId, startDateTime, endDateTime);
        }

        // GET api/<controller>/5
        public Booking Get(string id)
        {
            return _bookingManager.GetBookingById(id);
        }


        // POST api/<controller>
        public void Post([FromBody]Booking booking)
        {
            _bookingManager.AddNewBooking(booking);
        }

        // PUT api/<controller>/5
        public void Put(string id, [FromBody]Booking booking)
        {
            _bookingManager.UpdateBooking(id, booking);
        }

        // DELETE api/<controller>/5
        public void Delete(string id)
        {
            _bookingManager.DeleteBooking(id);
        }
    }
}
