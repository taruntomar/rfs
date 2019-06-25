using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarunLab.RFS.RoomBooking.DataLayer.Models.POCO;
using TarunLab.RFS.RoomBooking.DataLayer.Paging;
using TarunLab.RFS.RoomBooking.DataLayer.UnitOfWork.EF;

namespace TarunLab.RFS.RoomBooking.DataLayer.Repositories
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
