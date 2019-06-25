
using System;
using TarunLab.RFS.RoomBooking.DataLayer.Models.POCO;

namespace TarunLab.RFS.RoomBooking.DataLayer.Repositories
{
    public interface IBookingRepository
    {
        Booking GetBookingWithId(string id);
        
    }
}