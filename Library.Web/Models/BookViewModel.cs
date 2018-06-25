using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Library.Model;

namespace Library.Web.Models
{
    
    public class BookViewModel
    {
        public string Isbn { get; set; }
        public string BookTitle { get; set; }
        public string PublishYear { get; set; }
        public int Count { get; set; }
        public int ReservedCount { get; set; }
        public int InLibraryCount { get; set; }
        public List<AuthorViewModel> Authors { get; set; }
        public List<BookIdsViewModel> BookIds { get; set; }
    }

    public class ReserveBookViewModel
    {
        public int ReserveId { get; set; }
        public string UserName { get; set; }
        public BookStatus ReserveState { get; set; }
        public DateTime ReserveDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime? UserReturnedDate { get; set; }
    }

}