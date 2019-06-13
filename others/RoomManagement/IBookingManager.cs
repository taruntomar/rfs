using System.Collections.Generic;
using RoomManagement.Entities;

namespace RoomManagement
{
    public interface IBookingManager
    {
        bool CreateBooking();
        IEnumerable<Booking> GetTodayBooking();
        void UpdateBooking(int id, Booking booking);
        void DeleteBooking(string id);
        string GetBookingById(int id);
        void AddNewBooking(object bookingName);
    }
}