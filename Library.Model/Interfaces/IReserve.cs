using System;

namespace Library.Model
{
    public interface IReserve
    {
        DateTime CalculatedReturnDate { get; set; }
        DateTime DateReserved { get; set; }
        int ReserveId { get; set; }
        string BookId { get; set; }
        ApplicationUser User { get; set; }
        BookIds BookIds { get; set; }
        string UserId { get; set; }
        DateTime? UserReturnedDate { get; set; }
    }
}