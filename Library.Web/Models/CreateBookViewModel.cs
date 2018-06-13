using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library.Web.Models
{
    public class CreateBookViewModel
    {
        [Required]
        public string Isbn { get; set; }
        [Required]
        public string BookTitle { get; set; }
        [Required]
        public string PublishYear { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public List<AuthorViewModel> Author { get; set; }
    }

    public class AuthorViewModel
    {
        [Required]
        public string AuthorName { get; set; }
    }
}