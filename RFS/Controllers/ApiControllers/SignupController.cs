using DataLayer;
using DataLayer.Models;
using RFS.Models;
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
                string code = Guid.NewGuid().ToString();
                SendAccountVerificationEmail(c.username,code);
            }

            // show message to check inbox for activation link.

            return resp;
        }

        async Task SendAccountVerificationEmail(string email ,string code)
        {
            var apiKey = System.Configuration.ConfigurationManager.AppSettings["sendgridkey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("india-facility@resideo.com","Resideo India Operation");
            var subject = "Verify your account";
            var to = new EmailAddress(email, email);
            var plainTextContent = "";
            var htmlContent = "<div>Click on given link or Goto portal > user > my profile > verify account and enter the code: "+code+"</div><a href=\"http://localhost:64486/Identity/acticationcode="+ code + "\">Verify Account</a>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}