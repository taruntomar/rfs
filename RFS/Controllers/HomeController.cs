
using System;
using System.Collections.Generic;
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
                username = Server.HtmlEncode(myCookie.Value);
                if(Session[username].ToString() == "loggedIn")
                {
                    return View();
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