using System;

namespace Library.Model
{
    public interface IReserve
    {
        Books Books { get; set; }
        DateTime CalculatedReturnDate { get; set; }
        DateTime DateReserved { get; set; }
        string Isbn { get; set; }
        int ReserveId { get; set; }
        ApplicationUser User { get; set; }
        string UserId { get; set; }
        DateTime? UserReturnedDate { get; set; }
    }
}