
using RoomManagement;
using RoomManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RFS_API.Controllers
{
    [Authorize]
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
            string username = User.Identity.Name;
            
            return _locationManager.GetAllLocations();
        }

        // GET api/<controller>/5
        public Location Get(string id)
        {
            return _locationManager.GetLocationById(id);
        }

        // POST api/<controller>
        public HttpResponseMessage Post(HttpRequestMessage httpRequest, [FromBody]Location location)
        {
            string message =_locationManager.AddNewLocation(location);
            var response = httpRequest.CreateResponse(message);
            response.StatusCode = System.Net.HttpStatusCode.Created;
            return response;
        }

        // PUT api/<controller>/5
        public void Put(string id, [FromBody]Location location)
        {
            _locationManager.UpdateLocationProperties(id, location);
        }

        // DELETE api/<controller>/5
        public void Delete(string id)
        {
            _locationManager.DeleteLocation(id);
        }
    }
}
