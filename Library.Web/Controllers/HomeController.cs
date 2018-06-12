using System.Web.Mvc;
using Library.Service;

namespace Library.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService bookService;
        private readonly IReserveService reserveService;
        private readonly IAuthorService authorService;

        public HomeController(IBookService bookService, 
            IReserveService reserveService, IAuthorService authorService)
        {
            this.bookService = bookService;
            this.reserveService = reserveService;
            this.authorService = authorService;
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
            
        }
        public ActionResult Register()
        {
            ViewBag.show = "Register";
            return View();
        }

        public ActionResult Login()
        {
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
    }

  
}
