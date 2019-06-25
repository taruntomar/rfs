using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TarunLab.RFS.RoomBooking.DataLayer.Models.POCO;
using TarunLab.RFS.RoomBooking.DataLayer.Repositories;

namespace RoomBookingWebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private IBookingRepository _bookingRepository;
        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        // GET api/Bookings
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Booking1", "Booking2" };
        }

        // GET api/Bookings/5
        [HttpGet("{id}")]
        public ActionResult<Booking> Get(string id)
        {
            var booking = _bookingRepository.GetBookingWithId(id);
            if (booking == null)
            {
                //booking not found
                return NotFound();
            }
            return booking;
        }

        // POST api/Bookings
        [HttpPost]
        public void Post([FromBody] string Booking)
        {
        }

        // PUT api/Bookings/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string Booking)
        {
        }

        // DELETE api/Bookings/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
