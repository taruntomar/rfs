using System;
using TarunLab.RFS.RoomBooking.DataLayer.Repositories;
using TarunLab.RFS.RoomBooking.DataLayer.UnitOfWork;
using TarunLab.RFS.RoomBooking.DataLayer.UnitOfWork.EF;

namespace RoomBookingWebAPIs.DataLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private RFSDbContext _dbContext;
        private ILocationRepository _locationRepository = null;
        public UnitOfWork(RFSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ILocationRepository GetLocationRepository()
        {
            if (_locationRepository == null)
                _locationRepository = new LocationRepository(_dbContext);
            return _locationRepository;
        }
    }
}
