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
            return _userManager.GetAllUsers().Select<user,user>( (x,u) => { x.salt = ""; x.password = ""; x.logincode = ""; return x; });
        }

        // GET api/<controller>/5
        public user Get(string id)
        {
            var user=  _userManager.GetUserById(id);
            user.logincode = "";
            user.password = "";
            user.salt = "";

            return user;
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
    public class MeController : ApiController
    {
        IUserManager _userManager = null;
        public MeController(IUserManager userManager)
        {
            _userManager = userManager;
        }
        // GET api/<controller>
        public dynamic Get()
        {
            var username = new TApiAuth().GetLoggedInUsername(Request);
            if (string.IsNullOrEmpty(username))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            var user = _userManager.GetUserFromMailId(username);
            return new {Id =user.Id, Name=user.Name, Email = user.email, isAdmin = user.isAdmin, loc = new { Name = user.location }, Phone = user.phone, };
                
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/isadmin")]
        public bool IsAdmin()
        {
            var username = new TApiAuth().GetLoggedInUsername(Request);
            if (string.IsNullOrEmpty(username))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            var user = _userManager.GetUserFromMailId(username);
            return user.isAdmin.HasValue && user.isAdmin.Value ? true : false;

        }


        // PUT api/<controller>/5
        public void Put(string id, [FromBody]dynamic user)
        {
            var usr = _userManager.GetUserById(id);
            usr.Name = user.Name;
            usr.phone = user.Phone;
            usr.location = user.loc.Name;

            _userManager.UpdateUserProperties(id, usr);

        }

        // DELETE api/<controller>/5
        public void Delete(string id)
        {
            _userManager.DeleteUser(id);
        }
    }

}