using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Model;

namespace Library.Model
{
    public class Log : ILog
    {
        public int LogId { get; set; }
        public string RequestMethod { get; set; }
        public string RequestUri { get; set; }
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string UserHostName { get; set; }
        public string UserHostAddress { get; set; }
        public string UserAgent { get; set; }
        public string ResponseStatusCode { get; set; }
        public string ResponseError { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
