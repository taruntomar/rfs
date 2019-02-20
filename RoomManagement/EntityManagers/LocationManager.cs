
using RoomManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomManagement
{
    public class LocationManager : ILocationManager
    {
        private IRoomManagementDatabasseEntities _dbContext;
        public LocationManager(IRoomManagementDatabasseEntities dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddNewLocation(Location room)
        {
            _dbContext.Locations.Add(room);
            _dbContext.SaveChanges();
        }

        public void DeleteLocation(string id)
        {
            var location = _dbContext.Locations.FirstOrDefault(x => x.Id == id);
            if (location != null)
            {
                _dbContext.Locations.Remove(location);
                _dbContext.SaveChanges();
            }
          
        }

        public IList<Location> GetAllLocations()
        {
            return _dbContext.Locations.ToList();
        }

        public Location GetLocationById(string id)
        {
            var location = _dbContext.Locations.FirstOrDefault(x => x.Id == id);
            return location;
        }

        public void UpdateLocationProperties(string id, Location location)
        {
            var loc = _dbContext.Locations.FirstOrDefault(x => x.Id == id);
            loc.Name = location.Name;
            loc.Country = location.Country;
            _dbContext.SaveChanges();
        }
    }
}
