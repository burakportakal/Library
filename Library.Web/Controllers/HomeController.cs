using System.Web.Mvc;
using Library.Service;

namespace Library.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
            
        }
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");
            ViewBag.show = "Register";
            return View();
        }

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");
            ViewBag.Show = "Login";
            return View("Register");
        }
        public ActionResult RegisterPartial()
        {
            return PartialView("RegisterPartial");
        }

        public ActionResult LoginPartial()
        {
            return PartialView("LoginPartial");
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Index");
        }
        [Authorize]
        public new ActionResult Profile()
        {
            return View();
        }
    }

  
}
