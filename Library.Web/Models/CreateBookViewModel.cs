using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Web.Models
{
    public class CreateBookViewModel
    {
        public string Isbn { get; set; }
        public string BookTitle { get; set; }
        public string PublishYear { get; set; }
        public int Count { get; set; }
        public List<AuthorViewModel> Author { get; set; }
    }

    public class AuthorViewModel
    {
        public string AuthorName { get; set; }
    }
}