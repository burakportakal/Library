using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Web.Models
{
    public class ReserveViewModel : IReserveViewModel
    {
        public int ReserveId { get; set; }
        public string Isbn { get; set; }
        public string BookTitle { get; set; }
        public string UserId { get; set; }
        public DateTime ReserveDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime UserReturnedDate { get; set; }
    }
}