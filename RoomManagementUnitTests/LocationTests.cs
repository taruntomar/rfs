using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoomManagement;
using RoomManagement.Entities;

namespace RoomManagementUnitTests
{
    [TestClass]
    public class LocationManagementTests
    {
        [TestMethod]
        public void Test1_AddLocation()
        {
            IRoomManagementDatabasseEntities dbContext = new RoomManagementDatabasseEntities();
            LocationManager locationManager = new LocationManager(dbContext);

            var location= new Location() { Country = "India", Id = Guid.NewGuid().ToString(), Name = "Vidhan Soudha" };
            string message =  locationManager.AddNewLocation(location);
            var loc = locationManager.GetLocationById(location.Id);
            Assert.AreEqual(loc.Name == location.Name && loc.Country == location.Country, true);


        }

        [TestMethod]
        public void Test2_AddAgainSameLocation()
        {
            IRoomManagementDatabasseEntities dbContext = new RoomManagementDatabasseEntities();
            LocationManager locationManager = new LocationManager(dbContext);

            var location = new Location() { Country = "India", Id = Guid.NewGuid().ToString(), Name = "Vidhan Soudha" };
            string message = locationManager.AddNewLocation(location);
            var locs = locationManager.GetAllLocations();
            var l  = locs.Where(x => x.Name == location.Name && x.Country == location.Country);

            Assert.AreEqual(l.Count(), 1);
        }

        [TestMethod]
        public void Test3_RemoveLocation()
        {
            IRoomManagementDatabasseEntities dbContext = new RoomManagementDatabasseEntities();
            LocationManager locationManager = new LocationManager(dbContext);
            var location = new Location() { Country = "India", Id = Guid.NewGuid().ToString(), Name = "Vidhan Soudha" };
            var locs = locationManager.GetAllLocations();
            var l = locs.FirstOrDefault(x => x.Name == location.Name && x.Country == location.Country);
                locationManager.DeleteLocation(l.Id);

            locs = locationManager.GetAllLocations();
            l = locs.FirstOrDefault(x => x.Name == location.Name && x.Country == location.Country);
            Assert.AreEqual(l, null);
        }
    }
}
