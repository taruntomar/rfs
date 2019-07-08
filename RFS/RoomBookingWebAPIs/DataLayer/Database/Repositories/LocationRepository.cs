using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarunLab.RFS.RoomBooking.DataLayer.Models.POCO;
using TarunLab.RFS.RoomBooking.DataLayer.Paging;
using TarunLab.RFS.RoomBooking.DataLayer.UnitOfWork.EF;

namespace TarunLab.RFS.RoomBooking.DataLayer.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private RFSDbContext _dbContext;
        public LocationRepository(RFSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddLocation(string locationName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Location> GetAllLocations()
        {
            return _dbContext.Locations.AsEnumerable();
        }

        public PagedResult<Location> GetListOfLocations()
        {
            throw new NotImplementedException();
        }

        public Location GetLocationById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
