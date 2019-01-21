
using DataLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RFS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string username = "";
            HttpCookie myCookie = new HttpCookie("rfs.username");
            myCookie = Request.Cookies["rfs.username"];
            if (myCookie != null)
            {
                HttpCookie myCookie2 = new HttpCookie("rfs.logincode");
                myCookie2 = Request.Cookies["rfs.logincode"];
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
                        ViewBag.username = user;
                        return View();
                    }
                    
                    // redirect to home page

                    //username = Server.HtmlEncode(myCookie.Value);
                    //if (Session[username].ToString() == "loggedIn")
                    //{
                        
                    //}
                }
            }
            return RedirectToAction("Index", "Identity");

            //return new IdentityController().Index();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}