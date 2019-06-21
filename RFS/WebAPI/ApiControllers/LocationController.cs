using RFS.Models;
using WebAPI.Models.Entities;
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
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            var useremail = new TApiAuth().GetLoggedInUsername(Request);
            if (string.IsNullOrEmpty(useremail))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            var locations = _locationManager.GetAllLocations().Where(x=>(x.enabled.HasValue && x.enabled.Value));
            HttpResponseMessage response = request.CreateResponse<IEnumerable<Location>>(HttpStatusCode.OK, locations);
            return response;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/locations/manage")]
        public HttpResponseMessage GetForManage(HttpRequestMessage request)
        {
            var useremail = new TApiAuth().GetLoggedInUsername(Request);
            if (string.IsNullOrEmpty(useremail))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            var user = _userManager.GetUserFromMailId(useremail);
            if (user.isAdmin.HasValue && user.isAdmin.Value)
            {
                var locations = _locationManager.GetAllLocations();
                HttpResponseMessage response = request.CreateResponse<IEnumerable<Location>>(HttpStatusCode.OK, locations);
                return response;
            }
            else
            {
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.Unauthorized);
                return response;
            }
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

        public HttpResponseMessage Put(HttpRequestMessage httpRequest, string id, [FromBody]Location location)
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
                _locationManager.UpdateLocationProperties(id, location);
                var response = httpRequest.CreateResponse("Update successful");
                response.StatusCode = System.Net.HttpStatusCode.Created;
                return response;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            
        }

        // DELETE api/<controller>/5
        public void Delete(string id)
        {
            _locationManager.DeleteLocation(id);
        }
    }
}