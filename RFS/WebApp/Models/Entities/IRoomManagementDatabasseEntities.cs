using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using RFS.Models.Entities;

namespace RoomManagement.Entities
{
    public interface IRoomManagementDatabasseEntities
    {
         DbSet<Booking> Bookings { get; set; }
          DbSet<Location> Locations { get; set; }
         DbSet<Room> Rooms { get; set; }
         DbSet<RoomPicture> RoomPictures { get; set; }
        DbSet<user> users { get; set; }
        Database Database { get; }
        DbChangeTracker ChangeTracker { get; }
        DbContextConfiguration Configuration { get; }


        IEnumerable<DbEntityValidationResult> GetValidationErrors();

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync();

    }
}