using System;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Library.Model;
using Library.Service;

namespace Library.Web.Logger
{
    /// <summary>
    /// Her web api isteği önce burada işlenir.
    /// </summary>
    public class MessageHandler : DelegatingHandler
    {
        /// <summary>
        /// Her web api isteği burada Log tablosuna kaydedilir.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var logService = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILogService)) as ILogService;
            var context = request.GetOwinContext().Authentication;
            var response = await base.SendAsync(request, cancellationToken);
            if (response == null)
            {
                return null;
            }
            var isAuthenticated = context.User.Identity.IsAuthenticated;
            var userName = context.User.Identity.Name;
            var clientIp= GetClientIp(request);
            var log = Factory.GetLogInstance(userName,isAuthenticated,clientIp,request,response);
            logService.AddLog((Log)log);
            logService.SaveChanges();
            return response;
        }
        /// <summary>
        /// İstek yapan kişinin Ip adresini döndürür.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string GetClientIp(HttpRequestMessage request = null)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }
    }
}