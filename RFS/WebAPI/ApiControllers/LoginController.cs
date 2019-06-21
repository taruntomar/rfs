using System.Net.Http;
using System.Web.Http;
using DataLayer.Models;
using TAuthNIdentity;
using System.Web.Http.Cors;

namespace RFS.Controllers.ApiControllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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