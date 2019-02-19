using LocationManagement;
using RoomManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace RFS_API.Controllers
{
    public class LocationsController : ApiController
    {
        ILocationManager _locationManager = null;
        public LocationsController(ILocationManager locationManager)
        {
            _locationManager = locationManager;
        }
        // GET api/<controller>
        public IEnumerable<Location> Get()
        {

            return _locationManager.GetAllLocations();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return _locationManager.GetLocationById(id);
        }

        // POST api/<controller>
        public void Post([FromBody]Location location)
        {
            _locationManager.AddNewLocation(location);
            
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]Location location)
        {
            _locationManager.UpdateLocationProperties(id, location);
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            _locationManager.DeleteLocation(id);
        }
    }
}
