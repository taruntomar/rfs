using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomManagement.Entities;

namespace RoomManagement
{
    public class BookingManager : IBookingManager
    {
        public BookingManager()
        {

        }

        public void AddNewBooking(object bookingName)
        {
            throw new NotImplementedException();
        }

        public bool CreateBooking()
        {
            return true;
        }

        public void DeleteBooking(string id)
        {
            throw new NotImplementedException();
        }

        public string GetBookingById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Booking> GetTodayBooking()
        {
            throw new NotImplementedException();
        }

        public void UpdateBooking(int id, Booking booking)
        {
            throw new NotImplementedException();
        }
    }
}
