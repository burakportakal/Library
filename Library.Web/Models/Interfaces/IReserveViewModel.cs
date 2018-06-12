using System;

namespace Library.Web.Models
{
    public interface IReserveViewModel
    {
        int ReserveId { get; set; }
        string Isbn { get; set; }
        string BookTitle { get; set; }
        DateTime ReserveDate { get; set; }
        DateTime ReturnDate { get; set; }
        string UserId { get; set; }
        DateTime UserReturnedDate { get; set; }
    }
}