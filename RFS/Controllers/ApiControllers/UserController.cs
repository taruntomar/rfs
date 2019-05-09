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
    public class UserController : ApiController
    {
        IUserManager _userManager = null;
        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }
        // GET api/<controller>
        public IEnumerable<user> Get()
        {
            return _userManager.GetAllUsers();
        }

        // GET api/<controller>/5
        public user Get(string id)
        {
            return _userManager.GetUserById(id);
        }

        // POST api/<controller>
        public HttpResponseMessage Post(HttpRequestMessage httpRequest, [FromBody]user user)
        {
            string message = _userManager.AddNewUser(user);
            var response = httpRequest.CreateResponse(message);
            response.StatusCode = System.Net.HttpStatusCode.Created;
            return response;
        }

        // PUT api/<controller>/5
        public void Put(string id, [FromBody]user user)
        {
            _userManager.UpdateUserProperties(id, user);
        }

        // DELETE api/<controller>/5
        public void Delete(string id)
        {
            _userManager.DeleteUser(id);
        }
    }
}