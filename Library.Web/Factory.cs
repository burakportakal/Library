using Library.Model;
using Library.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Library.Web
{
    public static class Factory
    {
        public static IBooks GetBookInstance()
        {
            return new Books();
        }

        public static IAuthors GetAuthorInstance()
        {
            return new Authors();
        }

        public static IReserve GetReserveInstace(string userId, BookIds book)
        {
            return new Reserve
            {
                UserId = userId,
                BookIds = book
            };
        }
        public static IApplicationUser GetApplicationUserInstance()
        {
            return new ApplicationUser();
        }

        public static IReserveViewModel GetReserveViewModel()
        {
            return new ReserveViewModel();
        }

        public static ILog GetLogInstance(string userName,bool isAuthenticated,string clientIp,HttpRequestMessage request,HttpResponseMessage response)
        {
            Log log = new Log();
            log.IsAuthenticated = isAuthenticated;
            log.UserName = userName;
            log.UserHostName = request.Headers.Host;
            log.UserHostAddress = clientIp;
            log.UserAgent = request.Headers.UserAgent.ToString();
            log.RequestDate = DateTime.Now;
            log.RequestMethod = request.Method.Method;
            log.RequestUri = request.RequestUri.AbsoluteUri;
            log.ResponseError = response.ReasonPhrase;
            log.ResponseStatusCode = response.StatusCode.ToString();
            return log;
        }
    }
}