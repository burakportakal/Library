using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace Library.Web.Models
{
    public class CreateBookViewModel
    {
        [Required]
        [StringLength(13,ErrorMessage = "Isbn 13 Karakter uzunluğunda olmalıdır", MinimumLength = 13)]
        public string Isbn { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Kitap ismi 2 karakterden küçük 100 karakterden uzun olamaz", MinimumLength = 2)]
        public string BookTitle { get; set; }
        [Required]
        [StringLength(4, ErrorMessage = "Çıkış tarihi 4 karakter uzunluğunda olmalıdır örn:2016", MinimumLength = 4)]
        public string PublishYear { get; set; }
        [Required(ErrorMessage = "Adet kısmı gereklikdir")]
        public int Count { get; set; }
        [Required]
        public List<AuthorViewModel> Author { get; set; }
    }

    public class AuthorViewModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "Yazar ismi en çok 50 en az 3 karakter uzunluğunda olmalıdır", MinimumLength = 5)]
        public string AuthorName { get; set; }
    }
}