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
        public async Task<HttpResponseMessage> Signup([FromBody]dynamic userinfo)
        {
           
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

                await SendEmailExecute((string)userinfo.email, (string)userinfo.name, code);
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
            var to = new EmailAddress(email, "asdfasdf");
            var plainTextContent = "rewt";
            var htmlContent = "<div>Click on given link or Goto portal > user > my profile > verify account and enter the code: "+code+"</div><a href=\"http://localhost:64486/Identity/acticationcode="+ code + "\">Verify Account</a>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            try
            {
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {

            }
        }

        async Task SendEmailExecute(string email, string name, string code)
        {
            var apiKey = System.Configuration.ConfigurationManager.AppSettings["sendgridkey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("india-facility@resideo.com", "Meeting Room Booking");
            var subject = "Verify your account";
            var to = new EmailAddress(email, name);
            var plainTextContent = "Room: " + "asdfasdf" + ", " + "asdfasdf" + "-" + "asdfasdf";
            var htmlContent = "Verification Code: "+code +"<br/><a>Check on Facility Portal</a>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}