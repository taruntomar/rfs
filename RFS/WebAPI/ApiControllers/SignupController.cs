using DataLayer;
using DataLayer.Models;
using RFS.Models;
using RoomManagement;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using TAuthNIdentity;

namespace RFS.Controllers.ApiControllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SignupController : ApiController
    {
        private IUserManager _userManager;
        private EmailComManager _emailComManager;
        public SignupController(IUserManager userManager)
        {
            _userManager = userManager;
        }
        // GET api/<controller>
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> Signup([FromBody]dynamic userinfo)
        {
            var host = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host + ":" + Request.RequestUri.Port;
            _emailComManager = new EmailComManager(host);
            string connectionstring = ConfigurationManager.AppSettings["dbconnectionstring"];
            IdentityValidation idv = new IdentityValidation(connectionstring);
            var resp = new HttpResponseMessage();
            UserCredential userCredential = new UserCredential() { username = userinfo.email, password = userinfo.password };

            if (idv.UserExist(userCredential))
            {
                string error1 = "error: user already exist with same email id. Try resetting password.";
                resp.StatusCode = System.Net.HttpStatusCode.NotModified;
                resp.ReasonPhrase = error1;
            }
            else
            {
                //generating code for verification of email id 
                string code = Guid.NewGuid().ToString();

                // reegister user
                idv.RegisterUser(userinfo,code);

                resp.StatusCode = System.Net.HttpStatusCode.OK;
                resp.ReasonPhrase = "User successfully created.";

                await _emailComManager.SendAccountVerificationLink((string)userinfo.email, (string)userinfo.name, code);
            }

            // show message to check inbox for activation link.

            return resp;
        }

       
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/ResetPassowrd")]
        public async Task<HttpResponseMessage> ResetPassowrd([FromBody]dynamic userinfo)
        {
            var host = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host + ":" + Request.RequestUri.Port;
            _emailComManager = new EmailComManager(host);
            // verify passcode with db and reset password
            string connectionstring = ConfigurationManager.AppSettings["dbconnectionstring"];
            IdentityValidation idv = new IdentityValidation(connectionstring);
            var resp = new HttpResponseMessage();
            UserCredential userCredential = new UserCredential() { username = userinfo.email, password = userinfo.password };


            if(idv.CheckPasswordResetCode((string)userinfo.email, (string)userinfo.code))
            {
                //reset password
                idv.UpdatePassword((string)userinfo.email, (string)userinfo.password);
            }
         
            resp.StatusCode = System.Net.HttpStatusCode.OK;
             resp.ReasonPhrase = "done.";
            return resp;
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/VerifyAccount")]
        public async Task<HttpResponseMessage> VerifyAccount([FromBody]dynamic userinfo)
        {
            var host = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host + ":" + Request.RequestUri.Port;
            _emailComManager = new EmailComManager(host);
            // verify passcode with db and reset password
            string connectionstring = ConfigurationManager.AppSettings["dbconnectionstring"];
            IdentityValidation idv = new IdentityValidation(connectionstring);
            var resp = new HttpResponseMessage();
            UserCredential userCredential = new UserCredential() { username = userinfo.email, password = userinfo.password };


            if (idv.CheckAccountVerificationCode((string)userinfo.email, (string)userinfo.code))
            {
                //mark account as verified
                idv.SetVerifyAccount((string)userinfo.email, (string)userinfo.password);
                resp.StatusCode = System.Net.HttpStatusCode.OK;
                resp.ReasonPhrase = "done.";
                return resp;
            }

            resp.StatusCode = System.Net.HttpStatusCode.BadRequest;
            resp.ReasonPhrase = "Code incorrect.";
            return resp;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/SendVerificationMail")]
        public async Task<HttpResponseMessage> SendVerificationMail()
        {
            HttpResponseMessage response = Request.CreateResponse();
            var email = new TApiAuth().GetLoggedInUsername(Request);
            if (string.IsNullOrEmpty(email))
            {
                response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                return response;
            }
            else
            {
                //generating code for verification of email id 
                string code = Guid.NewGuid().ToString();
                var host = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host + ":" + Request.RequestUri.Port;
                _emailComManager = new EmailComManager(host);
                string connectionstring = ConfigurationManager.AppSettings["dbconnectionstring"];
                IdentityValidation idv = new IdentityValidation(connectionstring);
                idv.UpdateAccountActivationCode(email,code);
                var user = _userManager.GetUserFromMailId(email);
                //user.VerificationCode = code;
                //_userManager.UpdateUserProperties(user.Id, user);
                await _emailComManager.SendAccountVerificationLink((string)email, (string)user.Name, code);
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return response;
            }
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/SendPasswordResetLink")]
        public async Task<HttpResponseMessage> SendPasswordResetLink(string email)
        {
            var host = Request.RequestUri.Scheme + "://" + Request.RequestUri.Host + ":" + Request.RequestUri.Port;
            _emailComManager = new EmailComManager(host);
            // generate code
            string passResetCode = Guid.NewGuid().ToString();
            
            // store in database
            // verify passcode with db and reset password
            string connectionstring = ConfigurationManager.AppSettings["dbconnectionstring"];
            IdentityValidation idv = new IdentityValidation(connectionstring);
            idv.StorePasswordResetCode(email, passResetCode);
            await _emailComManager.SendPassowordResetLink((string)email, (string)"Dear User", passResetCode);
            var resp = new HttpResponseMessage();
            resp.StatusCode = System.Net.HttpStatusCode.OK;
                resp.ReasonPhrase = "password reset link has been send successfully.";
            return resp;

            //UserCredential userCredential = new UserCredential() { username = userinfo.email, password = userinfo.password };

            //if (idv.UserExist(userCredential))
            //{
            //    string error1 = "error: user already exist with same email id. Try resetting password.";
            //    resp.StatusCode = System.Net.HttpStatusCode.NotModified;
            //    resp.ReasonPhrase = error1;
            //}
            //else
            //{
            //    //generating code for verification of email id 
            //    string code = Guid.NewGuid().ToString();

            //    // reegister user
            //    idv.RegisterUser(userinfo, code);

            //    resp.StatusCode = System.Net.HttpStatusCode.OK;
            //    resp.ReasonPhrase = "User successfully created.";

            //    await SendEmailExecute((string)userinfo.email, (string)userinfo.name, code);
            //}

            // show message to check inbox for activation link.


        }
       
    }
}