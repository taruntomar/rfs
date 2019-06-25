using RoomBookingWebAPIs.Models;
using System;

namespace RoomBookingWebAPIs
{
    public interface IBookingRepository
    {
        Booking GetBookingWithId(string id);
        
    }
}