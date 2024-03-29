﻿using System.Configuration;
using System.Web.Mvc;

namespace RFS.Controllers
{
    public class IdentityController : Controller
    {
        public IdentityController()
        {
            ViewBag.Host = ConfigurationManager.AppSettings["webapihost"];
        }
        public ActionResult Index()
        {
            return View("Index", "~/Views/Shared/_LoginLayout.cshtml", null);
        }

        public ActionResult Signup()
        {
            ViewBag.Message = "Your application description page.";

            return View("Signup", "~/Views/Shared/_SignupLayout.cshtml", null);
        }
        public ActionResult ResetPassword()
        {
            ViewBag.Message = "Your application description page.";

            return View("ResetPassword", "~/Views/Shared/_SignupLayout.cshtml", null);
        }


        public ActionResult ResetPasswordForm(string email, string code)
        {

            ViewBag.code = code;
            ViewBag.email = email;
            return View("ResetPasswordForm", "~/Views/Shared/_SignupLayout.cshtml", null);
        }

        public ActionResult VerifyAccount(string email, string code)
        {

            ViewBag.code = code;
            ViewBag.email = email;
            return View("VerifyAccount", "~/Views/Shared/_SignupLayout.cshtml", null);
        }

    }



}