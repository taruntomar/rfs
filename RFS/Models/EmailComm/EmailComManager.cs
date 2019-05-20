using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using RFS.Models.EmailComm;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace RFS.Models
{
    public class EmailComManager
    {
        public string Host { get; set; }
        private SendGridClient _sendGridClient;
        private EmailAddress _from;

        public EmailComManager(string host)
        {
            Host = host;
            var apikey = System.Configuration.ConfigurationManager.AppSettings["sendgridkey"];
            _sendGridClient = new SendGridClient(apikey);
            _from = new EmailAddress("india-facility@resideo.com", "Resideo India Operation");
        }

        public async Task SendAccountVerificationLink(string email, string name, string code)
        {
            var subject = "Verify your account";
            var to = new EmailAddress(email, name);
            var plainTextContent = "Room: " + "asdfasdf" + ", " + "asdfasdf" + "-" + "asdfasdf";
            string actionUrl = Host + "/Identity/VerifyAccount?email=" + email + "&code=" + code;
            var templateManager = new TemplateManager();
            var html = await templateManager.GetAccountVerificatitonTemplateAsync(Host);
            html = html.Replace("{{name}}", name);
            html = html.Replace("{{action_url}}", actionUrl);
            var msg = MailHelper.CreateSingleEmail(_from, to, subject, plainTextContent, html);
            var response = await _sendGridClient.SendEmailAsync(msg);
        }
        public async Task SendPassowordResetLink(string email, string name, string code)
        {
            
            var subject = "Password reset link";
            var to = new EmailAddress(email, name);
            var plainTextContent = "Room: " + "asdfasdf" + ", " + "asdfasdf" + "-" + "asdfasdf";
            string actionUrl = Host + "/Identity/ResetPasswordForm?email=" + email + "&code=" + code;
            //var htmlContent = "Password reset Code: " + code + "<br/><a href=\"" + actionUrl + "\" >Click here</a> to reset your password.";
            var templateManager = new TemplateManager();
                var html =  await templateManager.GetResetPasswordTemplate(Host);
            html = html.Replace("{{name}}", name);
            html = html.Replace("{{action_url}}", actionUrl);
            var msg = MailHelper.CreateSingleEmail(_from, to, subject, plainTextContent, html);
            var response = await _sendGridClient.SendEmailAsync(msg);
        }
    }
}