using System;
using System.Collections.Generic;
using WebAPI.Models.Entities;
using RoomManagement.Entities;

namespace RoomManagement
{
    public interface IBookingManager
    {
        void AddNewBooking(Booking booking);
        bool CreateBooking();
        void DeleteBooking(string id);
        Booking GetBookingById(string id);
        IEnumerable<Booking> GetBookingCreatedBetween(DateTime starttime, DateTime endTime);
        IEnumerable<Booking> GetBookingDoneByUser(string userId, DateTime starttime, DateTime endTime);
        IEnumerable<Booking> GetBookingDoneByUser(string userId);
        IEnumerable<Booking> GetBookingForRoom(string roomId, DateTime starttime, DateTime endTime);
        void UpdateBooking(string id, Booking booking);
    }
}