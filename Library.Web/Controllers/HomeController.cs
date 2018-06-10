using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Model;
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
    }
}
