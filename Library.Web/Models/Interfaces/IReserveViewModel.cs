using System;
using System.Collections.Generic;

namespace Library.Web.Models
{
    public interface IReserveViewModel
    {
        int ReserveId { get; set; }
        string Isbn { get; set; }
        string BookTitle { get; set; }
        List<AuthorViewModel> Authors { get; set; }
        ReserveState ReserveState { get; set; }
        DateTime ReserveDate { get; set; }
        DateTime ReturnDate { get; set; }
        string UserId { get; set; }
        DateTime? UserReturnedDate { get; set; }
    }
}