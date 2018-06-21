using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Library.Model;
using Library.Service;

namespace Library.Web.Logger
{
    public class MessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var logService = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILogService)) as ILogService;
            var context = request.GetOwinContext().Authentication;
            var response = await base.SendAsync(request, cancellationToken);
            if (response == null)
                return null;
            var isAuthenticated = context.User.Identity.IsAuthenticated;
            var userName = context.User.Identity.Name;
            //await OutgoingMessageAsync(corrId, requestInfo, responseMessage);
            var log = Factory.GetLogInstance();
            log.IsAuthenticated= isAuthenticated;
            log.UserName = userName;
            log.UserHostName = request.Headers.Host;
            log.UserHostAddress = GetClientIp(request);
            log.UserAgent = request.Headers.UserAgent.ToString();
            log.RequestDate=DateTime.Now;
            log.RequestMethod = request.Method.Method;
            log.RequestUri = request.RequestUri.AbsoluteUri;
            log.ResponseError = response.ReasonPhrase;
            log.ResponseStatusCode = response.StatusCode.ToString();
            logService.AddLog((Log)log);
            logService.SaveChanges();
            return response;
        }
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