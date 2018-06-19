using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Web.Models
{
    public class BookIdsViewModel
    {
        public string BookId { get; set; }
        public string Status { get; set; }
        public List<ReserveViewModel> Reserves { get; set; }
    }
}