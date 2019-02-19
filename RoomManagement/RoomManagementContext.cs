using RoomManagement.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using Sstem.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomManagement
{
    public class RoomManagementContext: DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Room> Bookings { get; set; }
        public RoomManagementContext()
        {
            

        }
    }
}
