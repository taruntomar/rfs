using RFS.Models;
using WebAPI.Models.Entities;
using RoomManagement;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
            var users = _userManager.GetAllUsers();
            return users.Select(y => new  user{ Id = y.Id, email = y.email, IsActivated = y.IsActivated, isAdmin = y.isAdmin, IsVerified = y.IsVerified, location = y.location,  Name= y.Name , phone = y.phone});
            //return users.Select<user,user>( (x,u) => { x.salt = ""; x.password = ""; x.logincode = ""; return x; });
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/me/dp")]
        public HttpResponseMessage GetMyDP()
        {
            var useremail = new TApiAuth().GetLoggedInUsername(Request);
            if (string.IsNullOrEmpty(useremail))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            var user = _userManager.GetUserFromMailId(useremail);
            Byte[] b = user.UserProfilePics.FirstOrDefault().data;
            if (b == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(b));
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            return response;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/user/{userid}/dp")]
        public HttpResponseMessage GetDP(string userid)
        {
            var useremail = new TApiAuth().GetLoggedInUsername(Request);
            if (string.IsNullOrEmpty(useremail))
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            var user = _userManager.GetUserById(userid);
            if(user == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            var profilepic = user.UserProfilePics.FirstOrDefault();
            Byte[] b;
            if (profilepic == null)
            {
                string path = HttpContext.Current.Server.MapPath("~\\Content\\img\\user.png");
                int a = 1;
                b = File.ReadAllBytes(path);
                //using (FileStream fs = new FileStream(path, FileMode.Open))
                //{
                //    response.Content = new StreamContent(fs);
                //}
               
            }
            else
            {
                b = profilepic.data;
                
             
            }
            response.Content = new StreamContent(new MemoryStream(b));
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            return response;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/me/dp")]
        public async Task<IHttpActionResult> SetDP()
        {
            var useremail = new TApiAuth().GetLoggedInUsername(Request);
            if (string.IsNullOrEmpty(useremail))
            {
                return BadRequest();
            }
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();
                _userManager.SetUserProfilePic(useremail,buffer, filename);
            }

            return Ok();
        }

        // GET api/<controller>/5
        public user Get(string id)
        {
            var user=  _userManager.GetUserById(id);
            
            var tmp = new user { Id = user.Id, email = user.email, IsActivated = user.IsActivated, isAdmin = user.isAdmin, IsVerified = user.IsVerified, location = user.location, Name = user.Name, phone = user.phone };
            return tmp;
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

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("api/user/{userId}/activation")]
        public async Task<IHttpActionResult> SetUserActication(string userId,[FromBody]user u)
        {
            var useremail = new TApiAuth().GetLoggedInUsername(Request);
            if (string.IsNullOrEmpty(useremail))
            {
                return BadRequest();
            }
            var user = _userManager.GetUserFromMailId(useremail);
            if(user.isAdmin.HasValue && user.isAdmin.Value)
            {

                _userManager.SetUserActivation(_userManager.GetUserById(userId), u.IsActivated.HasValue?u.IsActivated.Value:false);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }

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
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/isVerified")]
        public bool isVerified()
        {
            var username = new TApiAuth().GetLoggedInUsername(Request);
            if (string.IsNullOrEmpty(username))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            var user = _userManager.GetUserFromMailId(username);
            return user.IsVerified.HasValue && user.IsVerified.Value ? true : false;

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