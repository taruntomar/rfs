using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace RFS.Controllers
{
    public class IdentityController : Controller
    {
        public ActionResult Index()
        {
            return View("Index", "~/Views/Shared/_LoginLayout.cshtml", null);
        }

        public ActionResult Signup()
        {
            ViewBag.Message = "Your application description page.";

            return View("Signup", "~/Views/Shared/_SignupLayout.cshtml", null);
        }
        [System.Web.Http.HttpPost]
        public ActionResult Login([FromBody]UserCredential c)
        {

            IdentityValidation idv = new IdentityValidation();

            if (idv.ValidateUserCredential(c))
            {
                // create cookie 
                HttpCookie myCookie = new HttpCookie("rfs.username");
                DateTime now = DateTime.Now;

                // Set the cookie value.
                myCookie.Value = c.username;
                // Set the cookie expiration date.
                myCookie.Expires = now.AddMinutes(30);

                // Add the cookie.
                Response.Cookies.Add(myCookie);
                //=========================================
                // create session
                Session[c.username] = "loggedIn";
                // redirect to home page
                RedirectToAction("Index", "Home");
            }


            string error = "Invalid Credential";
            return View("Index", error);

        }

        [System.Web.Http.HttpGet]
        public ActionResult Logout()
        {
            HttpCookie myCookie = new HttpCookie("rfs.username");
            myCookie = Request.Cookies["rfs.username"];
            if (myCookie != null)
            {
                Session[myCookie.Value] = "";
            }

            HttpCookie currentUserCookie = Request.Cookies["rfs.username"];
            Response.Cookies.Remove("rfs.username");
            currentUserCookie.Expires = DateTime.Now.AddDays(-10);
            currentUserCookie.Value = null;
            Response.SetCookie(currentUserCookie);
            //================================
            string message = "you have successfully logged out.";
            return View("Index", message);


        }
    }
}