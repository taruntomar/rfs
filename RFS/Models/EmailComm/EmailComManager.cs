using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        public async Task SendRoomBookingCalenderInvite(string recipientMail,string recipientName,string roomLocation,DateTime start, DateTime end)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("india-operation@resideo.com", "Resideo India Operation"),
                Subject = "Meeting Room Booking Confirmation",
                HtmlContent = "<strong>Hello, Email using HTML!</strong>"
            };
            var recipients = new List<EmailAddress>
            {
                new EmailAddress(recipientMail, recipientName)
            };
            msg.AddTos(recipients);

            string CalendarContent = MeetingRequestString("india-operations@resideo.com", new List<string>() { recipientMail }, "Meeting Room Booked", "Meeting room booking confirmation", roomLocation, start, end);
            byte[] calendarBytes = Encoding.UTF8.GetBytes(CalendarContent.ToString());
            SendGrid.Helpers.Mail.Attachment calendarAttachment = new SendGrid.Helpers.Mail.Attachment();
            calendarAttachment.Filename = "invite.ics";
            //the Base64 encoded content of the attachment.
            calendarAttachment.Content = Convert.ToBase64String(calendarBytes);
            calendarAttachment.Type = "text/calendar";
            msg.Attachments = new List<SendGrid.Helpers.Mail.Attachment>() { calendarAttachment };

            var response = await _sendGridClient.SendEmailAsync(msg);
        }


        private string MeetingRequestString(string from, List<string> toUsers, string subject, string desc, string location, DateTime startTime, DateTime endTime, int? eventID = null, bool isCancel = false)
        {
            StringBuilder str = new StringBuilder();

            str.AppendLine("BEGIN:VCALENDAR");
            str.AppendLine("PRODID:-//Microsoft Corporation//Outlook 12.0 MIMEDIR//EN");
            str.AppendLine("VERSION:2.0");
            str.AppendLine(string.Format("METHOD:{0}", (isCancel ? "CANCEL" : "REQUEST")));
            str.AppendLine("BEGIN:VEVENT");

            str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", startTime.ToUniversalTime()));
            str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmss}", DateTime.Now));
            str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", endTime.ToUniversalTime()));
            str.AppendLine(string.Format("LOCATION: {0}", location));
            str.AppendLine(string.Format("UID:{0}", (eventID.HasValue ? "blablabla" + eventID : Guid.NewGuid().ToString())));
            str.AppendLine(string.Format("DESCRIPTION:{0}", desc.Replace("\n", "<br>")));
            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", desc.Replace("\n", "<br>")));
            str.AppendLine(string.Format("SUMMARY:{0}", subject));

            str.AppendLine(string.Format("ORGANIZER;CN=\"{0}\":MAILTO:{1}", from, from));
            str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", string.Join(",", toUsers), string.Join(",", toUsers)));

            str.AppendLine("BEGIN:VALARM");
            str.AppendLine("TRIGGER:-PT15M");
            str.AppendLine("ACTION:DISPLAY");
            str.AppendLine("DESCRIPTION:Reminder");
            str.AppendLine("END:VALARM");
            str.AppendLine("END:VEVENT");
            str.AppendLine("END:VCALENDAR");

            return str.ToString();
        }
    }
}