using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Library.Model;
using Newtonsoft.Json;

namespace Library.Web.Controllers
{
    [Authorize(Roles="Admin")]
    public class LogController : Controller
    {
        // GET: Log
        public ActionResult Index()
        {
            var cookie = HttpContext.Request.Cookies["Token"];
            var obj = GetWebRequest();
            obj.Headers.Add("Authorization", "Bearer " + cookie.Value);

            using (var response = obj.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    var streamReader = new StreamReader(stream);
                    var text = streamReader.ReadToEnd();
                    var myObj = JsonConvert.DeserializeObject<IEnumerable<Log>>(text);
                    return View(myObj);
                }
            }
        }

        private WebRequest GetWebRequest()
        {
            if (Request.Url.Port.ToString() != "")
            {
                return WebRequest.Create("http://" + Request.Url.Host + ":" + Request.Url.Port + "/api/log");
            }
            else
            {
                return WebRequest.Create("http://" + Request.Url.Host +"/api/log");
            }
        }
    }
}