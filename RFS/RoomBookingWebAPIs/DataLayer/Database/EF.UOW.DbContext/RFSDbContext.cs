using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using TarunLab.RFS.RoomBooking.DataLayer.Models.POCO;

namespace TarunLab.RFS.RoomBooking.DataLayer.UnitOfWork.EF
{
    public class RFSDbContext : DbContext,IRFSDbContext 
    {

        public RFSDbContext(DbContextOptions<RFSDbContext> options) : base(options)
        {

        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Models.POCO.Location> Locations { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().ToTable("Rooms");
            modelBuilder.Entity<Models.POCO.Location>().ToTable("Locations");
            modelBuilder.Entity<Booking>().ToTable("Bookings");

            //base.OnModelCreating(modelBuilder);
        }
    }
}
