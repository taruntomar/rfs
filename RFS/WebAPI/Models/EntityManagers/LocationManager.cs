using WebAPI.Models.Entities;
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

        public string AddNewLocation(Location room)
        {
            var __room =  _dbContext.Locations.FirstOrDefault(x => x.Country == room.Country && x.Name == room.Name);
            if (__room == null)
            {
                _dbContext.Locations.Add(room);
                _dbContext.SaveChanges();
                return "Location added successfully.";
            }
            else
            {
                return "Location with same name and country already exist.";
            }
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
            List<Location> locations = new List<Location>();
            try
            {
                locations = _dbContext.Locations.ToList();
            }catch(Exception ex)
            {
                var asdf = ex;
            }
            return locations;
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
            loc.enabled = location.enabled;
            _dbContext.SaveChanges();
        }
    }
}
