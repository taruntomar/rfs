using RFS.Models;
using RFS.Models.Entities;
using RoomManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RFS.Controllers
{
    public class LocationsController : ApiController
    {
        ILocationManager _locationManager = null;
        IUserManager _userManager = null;
        public LocationsController(IUserManager userManager, ILocationManager locationManager)
        {
            _locationManager = locationManager;
            _userManager = userManager;
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
            var useremail = new TApiAuth().GetLoggedInUsername(Request);
            if (string.IsNullOrEmpty(useremail))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            var user = _userManager.GetUserFromMailId(useremail);
            if (user.isAdmin.HasValue && user.isAdmin.Value)
            {
                //check if user is admin
                location.Id = Guid.NewGuid().ToString();
                string message = _locationManager.AddNewLocation(location);
                var response = httpRequest.CreateResponse(message);
                response.StatusCode = System.Net.HttpStatusCode.Created;
                return response;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
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