
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Library.Web.Controllers
{
    [Authorize(Roles="Admin")]
    public class BookOperationsController : Controller
    {
        // GET: Bll
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult BookIndex()
        {
            return View();
        }
    }
}