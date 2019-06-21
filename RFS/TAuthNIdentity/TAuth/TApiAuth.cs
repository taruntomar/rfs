using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace TAuthNIdentity
{
    public class TApiAuth : ApiController
    {
        public void Logout(HttpRequestBase request, System.Web.HttpResponseBase response, System.Web.HttpSessionStateBase session)
        {
            //HttpCookie myCookie = new HttpCookie("rfs.username");
            //myCookie = Request.Cookies["rfs.username"];
            //if (myCookie != null)
            //{
            //    Session[myCookie.Value] = "";
            //}

            //HttpCookie currentUserCookie = Request.Cookies["rfs.username"];
            //Response.Cookies.Remove("rfs.username");
            //if (currentUserCookie != null)
            //{
            //    currentUserCookie.Expires = DateTime.Now.AddDays(-10);
            //    currentUserCookie.Value = null;
            //    Response.SetCookie(currentUserCookie);
            //}
        }

        public HttpResponseMessage Login(UserCredential c,System.Net.Http.HttpRequestMessage request)
        {

            var resp = new HttpResponseMessage();
            string connectionstring = ConfigurationManager.AppSettings["dbconnectionstring"];
            IdentityValidation idv = new IdentityValidation(connectionstring);
            if (idv.ValidateUserCredential(c))
            {
                var cookie = new CookieHeaderValue("rfs.username", c.username);
                // create cookie 
                cookie.Expires = DateTimeOffset.Now.AddMinutes(30);
                cookie.Domain = request.RequestUri.Host;
                cookie.Path = "/";

                // generate login-code
                string loginCode = Guid.NewGuid().ToString();
                idv.SetLoginCode(c.username, loginCode);

                var cookie2 = new CookieHeaderValue("rfs.logincode", loginCode);
                cookie2.Expires = DateTimeOffset.Now.AddMinutes(30);
                cookie2.Domain = request.RequestUri.Host;
                cookie2.Path = "/";
                resp.Headers.AddCookies(new CookieHeaderValue[] { cookie, cookie2 });
                resp.ReasonPhrase = "Login Successful.";
                resp.StatusCode = System.Net.HttpStatusCode.OK;
                //resp.Headers.Location = new Uri(HttpContext.Current.Request.Url.Authority);
            }
            else
            {
                string error = "Invalid Credential";
                resp.ReasonPhrase = error;
                resp.StatusCode = System.Net.HttpStatusCode.Unauthorized;
            }

          
            return resp;
        }
        //Following me
        public string GetLoggedInUsername(System.Net.Http.HttpRequestMessage request)
        {
            var myCookie=  request.Headers.GetCookies();
            if (myCookie != null && myCookie.Count!=0)
            {
                var username = myCookie.FirstOrDefault().Cookies.FirstOrDefault(c => c.Name == "rfs.username");
                var logincode = myCookie.FirstOrDefault().Cookies.FirstOrDefault(c => c.Name == "rfs.logincode");
                
                if((username!=null && !string.IsNullOrEmpty(username.Value)) && (logincode != null && !string.IsNullOrEmpty(logincode.Value)))
                {
                    string connectionstring = ConfigurationManager.AppSettings["dbconnectionstring"];
                    IdentityValidation idv = new IdentityValidation(connectionstring);
                    string user = HttpUtility.UrlDecode(username.Value);
                    string lc = HttpUtility.UrlDecode(logincode.Value);
                    if (idv.CheckLoginCode(user, lc))
                        return user;
                }
            }
            return string.Empty;
        }
    }
}