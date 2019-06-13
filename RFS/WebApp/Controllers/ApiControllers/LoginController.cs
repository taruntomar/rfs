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
    public class LoginController : ApiController
    {
        // GET api/<controller>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Login([FromBody]UserCredential c)
        {
            TApiAuth auth = new TApiAuth();
            return auth.Login(c, Request);

        }
    }
}