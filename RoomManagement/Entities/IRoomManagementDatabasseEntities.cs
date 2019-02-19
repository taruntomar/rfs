using System.Data.Entity;

namespace RoomManagement.Entities
{
    public interface IRoomManagementDatabasseEntities
    {
        DbSet<Booking> Bookings { get; set; }
        DbSet<Room> Rooms { get; set; }
    }
}