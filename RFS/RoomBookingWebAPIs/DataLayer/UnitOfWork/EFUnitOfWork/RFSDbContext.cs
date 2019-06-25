using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarunLab.RFS.RoomBooking.DataLayer.Models.POCO;

namespace TarunLab.RFS.RoomBooking.DataLayer.UnitOfWork.EF
{
    public class RFSDbContext : DbContext,IRFSDbContext 
    {

        public RFSDbContext() : base()
        {

        }

        public DbSet<Booking> Bookings { get; set; }
    }
}
