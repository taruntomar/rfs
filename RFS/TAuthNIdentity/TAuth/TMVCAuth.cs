using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace TAuthNIdentity
{
    public class TMVCAuth : Controller, ITAuth
    {
        public void Logout(System.Web.HttpRequestBase request, System.Web.HttpResponseBase response, System.Web.HttpSessionStateBase session)
        {
            HttpCookie myCookie = new HttpCookie("rfs.username");
            myCookie = request.Cookies["rfs.username"];
            if (myCookie != null)
            {
                session[myCookie.Value] = "";
            }

            HttpCookie currentUserCookie = request.Cookies["rfs.username"];
            response.Cookies.Remove("rfs.username");
            if (currentUserCookie != null)
            {
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                response.SetCookie(currentUserCookie);
            }
        }

        //Following me
        public string GetLoggedInUsername(System.Web.HttpRequestBase request)
        {
            HttpCookie myCookie = new HttpCookie("rfs.username");
            myCookie = request.Cookies["rfs.username"];
            if (myCookie != null)
            {
                HttpCookie myCookie2 = new HttpCookie("rfs.logincode");
                myCookie2 = request.Cookies["rfs.logincode"];
                if (myCookie2 != null)
                {
                    string connectionstring = ConfigurationManager.AppSettings["dbconnectionstring"];
                    IdentityValidation idv = new IdentityValidation(connectionstring);
                    string user = HttpUtility.UrlDecode(myCookie.Value);
                    string logincode = HttpUtility.UrlDecode(myCookie2.Value);
                    if (idv.CheckLoginCode(user, logincode))
                    {
                        // create session
                        //Session[myCookie.Value] = "loggedIn";
                        return user;

                    }

                    // redirect to home page

                    //username = Server.HtmlEncode(myCookie.Value);
                    //if (Session[username].ToString() == "loggedIn")
                    //{

                    //}
                }
            }
            return string.Empty;
        }

        public HttpResponseMessage Login(UserCredential c, System.Net.Http.HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }
    }
}