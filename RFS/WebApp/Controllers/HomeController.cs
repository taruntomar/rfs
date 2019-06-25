

using System.Configuration;
using System.Web.Mvc;
using TAuthNIdentity;

namespace RFS.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            ViewBag.Host = ConfigurationManager.AppSettings["webapihost"];
        }
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

        public ActionResult Admin()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult me()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult mybookings()
        {
            ViewBag.Message = "Your contact page.";
            
            return View();
        }
        public ActionResult roombooking()
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