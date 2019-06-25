using RoomBookingWebAPIs.Models;
using RoomBookingWebAPIs.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBookingWebAPIs
{
    public class BookingRepository: IBookingRepository
    {
        private RFSDbContext _rfsdbcontext;

        public BookingRepository(RFSDbContext rfsdbcontext)
        {
            _rfsdbcontext = rfsdbcontext;
        }

        public Booking GetBookingWithId(string id)
        {
            return _rfsdbcontext.Bookings.FirstOrDefault(x => x.Id == id);
        }

        public PagedResult<Booking> GetBooking(string roomId,string startDateTime, string endDateTime)
        {
            throw new NotImplementedException();
            //return _rfsdbcontext.Bookings.FirstOrDefault(x => x.Id == roomId);
        }
    }
}
