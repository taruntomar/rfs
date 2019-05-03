using DataLayer;
using RFS.Models;
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
            
            string username = new TMVCAuth().GetLoggedInUsername(Request);
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Index", "Identity");
            }
            else
            {
                ViewBag.username = username;
                return View();
            }

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

        [System.Web.Http.HttpGet]
        public ActionResult Logout()
        {
            new TMVCAuth().Logout(Request,Response,Session);
            //================================
            string message = "you have successfully logged out.";
            return RedirectToAction("Index", "Home");


        }


    }
}