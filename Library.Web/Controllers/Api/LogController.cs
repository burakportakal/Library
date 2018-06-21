using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Library.Service;

namespace Library.Web.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/log")]
    public class LogController : ApiController
    {
        private readonly ILogService logService;
        public LogController(ILogService logservice)
        {
            this.logService = logservice;
        }
        /// <summary>
        /// TÜm Log kayıtlarını getirir.
        /// </summary>
        /// <returns></returns>
        // GET: api/Log
        public HttpResponseMessage Get()
        {
            return  Request.CreateResponse(HttpStatusCode.OK, logService.GetAllLogs());
        }
    }
}
