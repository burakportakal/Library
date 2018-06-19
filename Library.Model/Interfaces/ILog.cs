using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public interface ILog
    {
        int LogId { get; set; }
        string RequestMethod { get; set; }
        string RequestUri { get; set; }
        bool IsAuthenticated { get; set; }
        string UserName { get; set; }
        string UserHostName { get; set; }
        string UserHostAddress { get; set; }
        string UserAgent { get; set; }
        string ResponseStatusCode { get; set; }
        string ResponseError { get; set; }
        DateTime RequestDate { get; set; }
    }
}
