using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBookingWebAPIs.Models
{
    public class RFSDbContext:DbContext
    {

        public RFSDbContext() : base()
        {

        }

        public DbSet<Booking> Bookings { get; set; }
    }
}
