using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RFS.Controllers
{
    public class IdentityController : Controller
    {
        public ActionResult Index()
        {
            return View("Index", "~/Views/Shared/_LoginLayout.cshtml",null);
        }

        public ActionResult Signup()
        {
            ViewBag.Message = "Your application description page.";

            return View("Signup", "~/Views/Shared/_SignupLayout.cshtml", null);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}