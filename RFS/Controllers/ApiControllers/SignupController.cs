using DataLayer;
using DataLayer.Models;
using RFS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
namespace RFS.Controllers.ApiControllers
{


    public class SignupController : ApiController
    {
        // GET api/<controller>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Signup([FromBody]UserCredential c)
        {
            string connectionstring = ConfigurationManager.AppSettings["dbconnectionstring"];
            IdentityValidation idv = new IdentityValidation(connectionstring);
            var resp = new HttpResponseMessage();

            if (idv.UserExist(c))
            {
                string error1 = "error: user already exist with same email id. Try resetting password.";
                resp.StatusCode = System.Net.HttpStatusCode.NotModified;
                resp.ReasonPhrase = error1;
            }
            else
            {
                // reegister user
                idv.RegisterUser(c);

                resp.StatusCode = System.Net.HttpStatusCode.OK;
                resp.ReasonPhrase = "User successfully created.";
            }

            // show message to check inbox for activation link.

            return resp;
        }
    }
}