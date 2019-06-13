using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RFS.Models.Entities;
using RoomManagement.Entities;

namespace RoomManagement
{
    public class BookingManager : IBookingManager
    {
        private IRoomManagementDatabasseEntities _dbContext;
        public BookingManager(IRoomManagementDatabasseEntities dbContext)
        {
            _dbContext = dbContext;
        }
   
        public void AddNewBooking(Booking booking)
        {
            _dbContext.Bookings.Add(booking);
            _dbContext.SaveChanges();

        }

        public bool CreateBooking()
        {
            return true;
        }

        public void DeleteBooking(string id)
        {
            Booking booking = GetBookingById(id);
            if (booking != null)
            {
                booking.isCancelled = true;
                _dbContext.SaveChanges();
            }
        }

        public Booking GetBookingById(string id)
        {
            return _dbContext.Bookings.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Booking> GetBookingForRoom(string roomId,DateTime starttime,DateTime endTime)
        {
            return _dbContext.Bookings.Where(x => x.RoomId == roomId && (x.starttime.CompareTo(starttime)>=0) && (x.endtime.CompareTo(endTime)<=0));
        }

        public IEnumerable<Booking> GetBookingCreatedBetween(DateTime starttime, DateTime endTime)
        {
            return _dbContext.Bookings.Where(x => (x.starttime.CompareTo(starttime) >= 0) && (x.endtime.CompareTo(endTime) <= 0));
        }

        public IEnumerable<Booking> GetBookingDoneByUser(string userId, DateTime starttime, DateTime endTime)
        {
            return _dbContext.Bookings.Where(x => x.createdBy == userId && (x.starttime.CompareTo(starttime) >= 0) && (x.endtime.CompareTo(endTime) <= 0));
        }

        public IEnumerable<Booking> GetBookingDoneByUser(string userId)
        {
            return _dbContext.Bookings.Where(x => x.createdBy == userId );
        }
        public void UpdateBooking(string id, Booking booking)
        {
            Booking __booking = GetBookingById(id);
            if (booking != null)
            {
                __booking.starttime = booking.starttime;
                __booking.endtime = booking.endtime;
                __booking.RoomId = booking.RoomId;
                _dbContext.SaveChanges();
            }
        }
    }
}
